using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Hosting;
using Xunit;
#if IOS || MACCATALYST
using NavigationViewHandler = Microsoft.Maui.Controls.Handlers.Compatibility.NavigationRenderer;
#endif

namespace Microsoft.Maui.DeviceTests
{
	[Category(TestCategory.ScrollView)]
	public partial class ScrollViewTests : ControlsHandlerTestBase
	{
		[Theory]
		[InlineData(ScrollOrientation.Vertical)]
		[InlineData(ScrollOrientation.Both)]
		public async Task TestContentSizeChangedHorizontal(ScrollOrientation orientation)
		{
			var handler = await SetUpScrollView(orientation);
			var scroll = handler.VirtualView as ScrollView;
			var changed = WatchContentSizeChanged(scroll);

			await AttachAndRun(scroll, async (handler) =>
			{
				var expectedSize = new Size(100, 100);
				await AssertContentSize(() => scroll.ContentSize, expectedSize);

				scroll.Content.WidthRequest = 200;
				await AssertContentSizeChanged(changed);

				expectedSize = new Size(200, 100);
				await AssertContentSize(() => scroll.ContentSize, expectedSize);
			});
		}

		[Theory]
		[InlineData(ScrollOrientation.Vertical)]
		[InlineData(ScrollOrientation.Both)]
		public async Task TestContentSizeChangedVertical(ScrollOrientation orientation)
		{
			var handler = await SetUpScrollView(orientation);
			var scroll = handler.VirtualView as ScrollView;
			var changed = WatchContentSizeChanged(scroll);

			await AttachAndRun(scroll, async (handler) =>
			{
				var expectedSize = new Size(100, 100);
				await AssertContentSize(() => scroll.ContentSize, expectedSize);

				scroll.Content.HeightRequest = 200;
				await AssertContentSizeChanged(changed);

				expectedSize = new Size(100, 200);
				await AssertContentSize(() => scroll.ContentSize, expectedSize);
			});
		}

		[Theory]
		[InlineData(ScrollOrientation.Vertical, 100, 300, 0, 100)]
		[InlineData(ScrollOrientation.Horizontal, 0, 100, 100, 300)]
		[InlineData(ScrollOrientation.Both, 100, 300, 100, 300)]
		public async Task TestScrollContentMargin(ScrollOrientation orientation, int verticalMargin,
			int expectedHeight, int horizontalMargin, int expectedWidth)
		{
			var handler = await SetUpScrollView(orientation, verticalMargin: verticalMargin, horizontalMargin: horizontalMargin);
			var scroll = handler.VirtualView as ScrollView;


			await AttachAndRun(scroll, async (handler) =>
			{
				var expectedSize = new Size(expectedWidth, expectedHeight);
				await AssertContentSize(() => scroll.ContentSize, expectedSize);
			});
		}

		// NOTE: this test is slightly different than MemoryTests.HandlerDoesNotLeak
		// It calls CreateHandlerAndAddToWindow(), a valid test case.
		[Fact(DisplayName = "ScrollView Does Not Leak")]
		public async Task DoesNotLeak()
		{
			SetupBuilder();
			WeakReference viewReference = null;
			WeakReference handlerReference = null;
			WeakReference platformReference = null;

			{
				var view = new Microsoft.Maui.Controls.ScrollView();
				var page = new ContentPage { Content = view };
				await CreateHandlerAndAddToWindow(page, () =>
				{
					viewReference = new(view);
					handlerReference = new(view.Handler);
					platformReference = new(view.Handler.PlatformView);
					page.Content = null;
				});
			}

			await AssertionExtensions.WaitForGC(viewReference, handlerReference, platformReference);
			Assert.False(viewReference.IsAlive, "View should not be alive!");
			Assert.False(handlerReference.IsAlive, "Handler should not be alive!");
			Assert.False(platformReference.IsAlive, "PlatformView should not be alive!");
		}

		[Fact(DisplayName = "Test call ScrollToAsync within NavigatedTo event")]
		public async Task TestCallScrollToAsyncWithinNavigatedToEvent()
		{
			SetupBuilder();
			var page1 = new ContentPage();
			var page2 = new ContentPage();
			var page3 = new ContentPage();
			var scrollView = new ScrollView()
			{
				Orientation = ScrollOrientation.Vertical,
				Content = new Grid
				{
					WidthRequest = 300,
					HeightRequest = 1000
				}
			};
			page2.Content = scrollView;
			page2.NavigatedTo += async (_, __) =>
			{
				double expectedPositionX = 0;
				double expectedPositionY = 200;
				await scrollView.ScrollToAsync(expectedPositionX, expectedPositionY, true);
				Assert.Equal(expectedPositionX, scrollView.ScrollX);
				Assert.Equal(expectedPositionY, scrollView.ScrollY);
			};
			var navPage = new NavigationPage(page1);

			await CreateHandlerAndAddToWindow<NavigationViewHandler>(navPage, async (handler) =>
			{
				await page1.Navigation.PushAsync(page2);
				await page2.Navigation.PushAsync(page3);
				await page3.Navigation.PopAsync();
			});
		}

		[Fact(DisplayName = "Test call ScrollToAsync within Appearing event")]
		public async Task TestCallScrollToAsyncWithinAppearingEvent()
		{
			SetupBuilder();
			var page1 = new ContentPage();
			var page2 = new ContentPage();
			var page3 = new ContentPage();
			var scrollView = new ScrollView()
			{
				Orientation = ScrollOrientation.Vertical,
				Content = new Grid
				{
					WidthRequest = 300,
					HeightRequest = 1000
				}
			};
			page2.Content = scrollView;
			page2.Appearing += async (_, __) =>
			{
				double expectedPositionX = 0;
				double expectedPositionY = 200;
				await scrollView.ScrollToAsync(expectedPositionX, expectedPositionY, true);
				Assert.Equal(expectedPositionX, scrollView.ScrollX);
				Assert.Equal(expectedPositionY, scrollView.ScrollY);
			};
			var navPage = new NavigationPage(page1);

			await CreateHandlerAndAddToWindow<NavigationViewHandler>(navPage, async (handler) =>
			{
				await page1.Navigation.PushAsync(page2);
				await page2.Navigation.PushAsync(page3);
				await page3.Navigation.PopAsync();
			});
		}

		void SetupBuilder()
		{
			EnsureHandlerCreated(builder =>
			{
				builder.ConfigureMauiHandlers(handlers =>
				{
					handlers.AddHandler(typeof(Toolbar), typeof(ToolbarHandler));
#if IOS || MACCATALYST
					handlers.AddHandler(typeof(NavigationPage), typeof(NavigationViewHandler));
#else
					handlers.AddHandler(typeof(NavigationPage), typeof(NavigationViewHandler));
#endif
					handlers.AddHandler<Page, PageHandler>();
					handlers.AddHandler<Label, LabelHandler>();
					handlers.AddHandler<IScrollView, ScrollViewHandler>();
					handlers.AddHandler<Grid, LayoutHandler>();
				});
			});
		}

		static async Task AssertContentSizeChanged(Task<bool> changed)
		{
			await WaitAssert(() => changed.IsCompleted && changed.Result, timeout: 5000, message: "PropertyChanged event with PropertyName 'ContentSize' did not fire").ConfigureAwait(false);
		}

		static async Task AssertContentSize(Func<Size> actual, Size expected)
		{
			await WaitAssert(() => CloseEnough(actual(), expected, 0.2), timeout: 5000, message: $"ContentSize was {actual()}, expected {expected}");
		}

		static bool CloseEnough(Size a, Size b, double tolerance)
		{
			if (System.Math.Abs(a.Width - b.Width) > tolerance)
			{
				return false;
			}

			if (System.Math.Abs(a.Height - b.Height) > tolerance)
			{
				return false;
			}

			return true;
		}

		static Task<bool> WatchContentSizeChanged(ScrollView scrollView)
		{
			var tcs = new TaskCompletionSource<bool>();

			void handler(object sender, PropertyChangedEventArgs args)
			{
				if (args.PropertyName == "ContentSize")
				{
					scrollView.PropertyChanged -= handler;
					tcs.SetResult(true);
				}
			}

			scrollView.PropertyChanged += handler;
			return tcs.Task;
		}

		async Task<ScrollViewHandler> SetUpScrollView(ScrollOrientation orientation, int horizontalMargin = 0, int verticalMargin = 0)
		{
			var view = new Label
			{
				WidthRequest = 100,
				HeightRequest = 100,
				Text = "Hello",
				BackgroundColor = Colors.LightBlue,
				Margin = new Thickness(horizontalMargin, verticalMargin)
			};

			var scroll = new ScrollView
			{
				Orientation = orientation,
				Content = view
			};

			var labelHandler = await CreateHandlerAsync<LabelHandler>(view);
			return await CreateHandlerAsync<ScrollViewHandler>(scroll);
		}

		static async Task WaitAssert(Func<bool> predicate, int interval = 100, int timeout = 1000, string message = "")
		{
			System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
			watch.Start();

			bool success = predicate();

			while (!success && watch.ElapsedMilliseconds < timeout)
			{
				await Task.Delay(interval).ConfigureAwait(false);
				success = predicate();
			}

			if (!success)
			{
				Assert.Fail(message);
			}
		}
	}
}

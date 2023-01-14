﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Java.Lang;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Xunit;

namespace Microsoft.Maui.DeviceTests
{
	public partial class ModalTests : ControlsHandlerTestBase
	{
		[Fact]
		public async Task ModalPageMarginCorrectAfterKeyboardOpens()
		{
			SetupBuilder();

			var navPage = new NavigationPage(new ContentPage());

			await CreateHandlerAndAddToWindow<IWindowHandler>(new Window(navPage),
				async (handler) =>
				{
					VerticalStackLayout layout = new VerticalStackLayout();
					List<Entry> entries = new List<Entry>();
					ContentPage modalPage = new ContentPage()
					{
						Content = layout
					};

					for (int i = 0; i < 30; i++)
					{
						var entry = new Entry();
						entries.Add(entry);
						layout.Add(entry);
					}

					await navPage.CurrentPage.Navigation.PushModalAsync(modalPage);
					await OnLoadedAsync(entries[0]);

					var pageBoundingBox = modalPage.GetBoundingBox();

					Entry testEntry = entries[0];
					foreach (var entry in entries)
					{
						var entryBox = entry.GetBoundingBox();

						// Locate the lowest visible entry
						if ((entryBox.Y + (entryBox.Height * 2)) > pageBoundingBox.Height)
							break;

						testEntry = entry;
					}

					await AssertionExtensions.HideKeyboardForView(testEntry);
					var rootPageOffsetY = navPage.CurrentPage.GetLocationOnScreen().Value.Y;
					var modalOffsetY = modalPage.GetLocationOnScreen().Value.Y;

					await AssertionExtensions.ShowKeyboardForView(testEntry);

					// Type text into the entries
					testEntry.Text = "Typing";

					// Validate that the correct offset was maintained when the window pan adjusted
					bool offsetMatchesWhenKeyboardOpened = await AssertionExtensions.Wait(() =>
					{
						var keyboardOpenRootPageOffsetY = navPage.CurrentPage.GetLocationOnScreen().Value.Y;
						var keyboardOpenModalOffsetY = modalPage.GetLocationOnScreen().Value.Y;

						var originalDiff = Math.Abs(rootPageOffsetY - modalOffsetY);
						var openDiff = Math.Abs(keyboardOpenRootPageOffsetY - keyboardOpenModalOffsetY);


						return Math.Abs(originalDiff - openDiff) <= 0.2;
					});

					Assert.True(offsetMatchesWhenKeyboardOpened, "Modal page has an invalid offset when open");

					await AssertionExtensions.HideKeyboardForView(testEntry);

					bool offsetMatchesWhenKeyboardClosed = await AssertionExtensions.Wait(() =>
					{
						var keyboardClosedRootPageOffsetY = navPage.CurrentPage.GetLocationOnScreen().Value.Y;
						var keyboardClosedModalOffsetY = modalPage.GetLocationOnScreen().Value.Y;

						return rootPageOffsetY == keyboardClosedRootPageOffsetY &&
								modalOffsetY == keyboardClosedModalOffsetY;
					});

					Assert.True(offsetMatchesWhenKeyboardClosed, "Modal page failed to return to expected offset");
				});
		}
	}
}

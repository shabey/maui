#nullable enable
using System;
using Microsoft.Maui.Graphics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WRect = Windows.Foundation.Rect; 

namespace Microsoft.Maui.Controls.Platform
{
	public class ViewToHandlerConverter : Microsoft.UI.Xaml.Data.IValueConverter
	{
		public object? Convert(object value, Type targetType, object parameter, string language)
		{
			var view = value as View;
			if (view == null)
			{
				var page = value as Page;
				if (page != null)
				{
					return page.ToPlatform(page.FindMauiContext()!);
				}
			}

			if (view == null)
				return null;

			return new WrapperControl(view);
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotSupportedException();
		}

		internal class WrapperControl : Panel
		{
			readonly View _view;
			IView View => _view;
			IPlatformViewHandler? Handler => View.Handler as IPlatformViewHandler;

			FrameworkElement FrameworkElement { get; }

			internal void CleanUp()
			{
				_view?.Cleanup();

				if(_view != null)
					_view.MeasureInvalidated -= OnMeasureInvalidated;
			}

			public WrapperControl(View view)
			{
				_view = view;
				_view.MeasureInvalidated += OnMeasureInvalidated;

				FrameworkElement = view.ToPlatform(view.FindMauiContext()!);
				Children.Add(FrameworkElement);

				// make sure we re-measure once the template is applied
				
				FrameworkElement.Loaded += (sender, args) =>
				{
					// If the view is a layout (stacklayout, grid, etc) we need to trigger a layout pass
					// with all the controls in a consistent native state (i.e., loaded) so they'll actually
					// have Bounds set
					Handler?.PlatformView?.InvalidateMeasure(View);
					InvalidateMeasure();
				};
			}

			void OnMeasureInvalidated(object? sender, EventArgs e)
			{
				InvalidateMeasure();
			}

			protected override global::Windows.Foundation.Size ArrangeOverride(global::Windows.Foundation.Size finalSize)
			{
				_view.IsInPlatformLayout = true;
				_view.Frame = new Rectangle(0, 0, finalSize.Width, finalSize.Height);
				FrameworkElement?.Arrange(new WRect(0, 0, finalSize.Width, finalSize.Height));

				if (_view.Width <= 0 || _view.Height <= 0)
				{
					// Hide Panel when size _view is empty.
					// It is necessary that this element does not overlap other elements when it should be hidden.
					Opacity = 0;
				}
				else
				{
					Opacity = 1;
				}

				_view.IsInPlatformLayout = false;

				return finalSize;
			}

			protected override global::Windows.Foundation.Size MeasureOverride(global::Windows.Foundation.Size availableSize)
			{
				FrameworkElement.Measure(availableSize);

				var request = FrameworkElement.DesiredSize;

				if (request.Height < 0)
				{
					request.Height = availableSize.Height;
				}

				global::Windows.Foundation.Size result;
				if (_view.HorizontalOptions.Alignment == LayoutAlignment.Fill && !double.IsInfinity(availableSize.Width) && availableSize.Width != 0)
				{
					result = new global::Windows.Foundation.Size(availableSize.Width, request.Height);
				}
				else
				{
					result = request;
				}

				return result;
			}
		}
	}
}
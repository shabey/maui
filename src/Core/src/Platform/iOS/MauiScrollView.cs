using System;
using System.Diagnostics.CodeAnalysis;
using CoreGraphics;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using UIKit;

namespace Microsoft.Maui.Platform
{
	public class MauiScrollView : UIScrollView, IUIViewLifeCycleEvents, IMauiPlatformView
	{
		bool _arranged;
		bool _invalidateParentWhenMovedToWindow;
		double _lastMeasureHeight;
		double _lastMeasureWidth;

		WeakReference<IScrollView>? _reference;

		internal IScrollView? View
		{
			get => _reference != null && _reference.TryGetTarget(out var v) ? v : null;
			set => _reference = value == null ? null : new(value);
		}

		// overriding this method so it does not automatically scroll large UITextFields
		// while the KeyboardAutoManagerScroll is scrolling.
		public override void ScrollRectToVisible(CGRect rect, bool animated)
		{
			if (!KeyboardAutoManagerScroll.IsKeyboardAutoScrollHandling)
				base.ScrollRectToVisible(rect, animated);
		}

#pragma warning disable RS0016
		public override void LayoutSubviews()
#pragma warning restore RS0016
		{
			// LayoutSubviews is invoked while scrolling, so we need to arrange the content only when it's necessary.
			// This could be done via `override ScrollViewHandler.PlatformArrange` but that wouldn't cover the case
			// when the ScrollView is attached to a non-MauiView parent (i.e. DeviceTests).
			if (!_arranged && View is { } scrollView)
			{
				var bounds = Bounds;
				var widthConstraint = (double)bounds.Width;
				var heightConstraint = (double)bounds.Height;

				// If the SuperView is a MauiView (backing a cross-platform ContentView or Layout), then measurement
				// has already happened via SizeThatFits and doesn't need to be repeated in LayoutSubviews. But we
				// _do_ need LayoutSubviews to make a measurement pass if the parent is something else (for example,
				// the window); there's no guarantee that SizeThatFits has been called in that case.
				if (!IsMeasureValid(widthConstraint, heightConstraint) && Superview is not MauiView)
				{
					CrossPlatformMeasure(scrollView, widthConstraint, heightConstraint);
					CacheMeasureConstraints(widthConstraint, heightConstraint);
				}

				var size = scrollView.ArrangeContentUnbounded(new Rect(new Point(), bounds.Size.ToSize()));
				ContentSize = size.ToCGSize();
			}

			base.LayoutSubviews();
		}

#pragma warning disable RS0016
		public override CGSize SizeThatFits(CGSize size)
#pragma warning restore RS0016
		{
			if (View is not { } scrollView)
			{
				return new CGSize();
			}

			var widthConstraint = (double)size.Width;
			var heightConstraint = (double)size.Height;

			var measuredSize = CrossPlatformMeasure(scrollView, widthConstraint, heightConstraint);
			CacheMeasureConstraints(widthConstraint, heightConstraint);

			return measuredSize;
		}

		static Size CrossPlatformMeasure(IScrollView scrollView, double widthConstraint, double heightConstraint)
		{
			if (scrollView.Orientation is ScrollOrientation.Horizontal or ScrollOrientation.Both)
			{
				widthConstraint = double.PositiveInfinity;
			}

			if (scrollView.Orientation is ScrollOrientation.Vertical or ScrollOrientation.Both)
			{
				heightConstraint = double.PositiveInfinity;
			}

			var measuredSize = scrollView.MeasureContent(scrollView.Padding, widthConstraint, heightConstraint);
			return measuredSize;
		}

		bool IsMeasureValid(double widthConstraint, double heightConstraint)
		{
			// Check the last constraints this View was measured with; if they're the same,
			// then the current measure info is already correct, and we don't need to repeat it
			return heightConstraint == _lastMeasureHeight && widthConstraint == _lastMeasureWidth;
		}

		void InvalidateConstraintsCache()
		{
			_lastMeasureWidth = double.NaN;
			_lastMeasureHeight = double.NaN;
		}

		void CacheMeasureConstraints(double widthConstraint, double heightConstraint)
		{
			_lastMeasureWidth = widthConstraint;
			_lastMeasureHeight = heightConstraint;
		}

		void IMauiPlatformView.InvalidateAncestorsMeasuresWhenMovedToWindow()
		{
			_invalidateParentWhenMovedToWindow = true;
		}

		void IMauiPlatformView.InvalidateMeasure(bool isPropagating)
		{
			SetNeedsLayout();
			InvalidateConstraintsCache();
			_arranged = false;

			var contentSize = ContentSize;
			var bounds = Bounds;

			if (isPropagating && (contentSize.Width <= bounds.Width || contentSize.Height <= bounds.Height))
			{
				// If the current content size is larger than the bounds, it means we're already scrolling.
				// So there's no reason to invalidate the ancestors measures and rearrange the scrollview.
				this.InvalidateAncestorsMeasures();
			}
		}

		[UnconditionalSuppressMessage("Memory", "MEM0002", Justification = IUIViewLifeCycleEvents.UnconditionalSuppressMessage)]
		EventHandler? _movedToWindow;

		event EventHandler IUIViewLifeCycleEvents.MovedToWindow
		{
			add => _movedToWindow += value;
			remove => _movedToWindow -= value;
		}

		public override void MovedToWindow()
		{
			base.MovedToWindow();
			_movedToWindow?.Invoke(this, EventArgs.Empty);
			if (_invalidateParentWhenMovedToWindow)
			{
				_invalidateParentWhenMovedToWindow = false;
				this.InvalidateAncestorsMeasures();
			}
		}
	}
}


﻿using System;
using CoreAnimation;
using CoreGraphics;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;

namespace Microsoft.Maui.Platform
{
	public class ContentView : MauiView
	{
		WeakReference<IBorderStroke>? _clip;
		CAShapeLayer? _childMaskLayer;
		internal event EventHandler? LayoutSubviewsChanged;

		public override CGSize SizeThatFits(CGSize size)
		{
			if (CrossPlatformMeasure == null)
			{
				return base.SizeThatFits(size);
			}

			var widthConstraint = size.Width;
			var heightConstraint = size.Height;

			var crossPlatformSize = CrossPlatformMeasure(widthConstraint, heightConstraint);

			CacheMeasureConstraints(widthConstraint, heightConstraint);

			return crossPlatformSize.ToCGSize();
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			var bounds = AdjustForSafeArea(Bounds).ToRectangle();
			var widthConstraint = bounds.Width;
			var heightConstraint = bounds.Height;

			// If the SuperView is a MauiView (backing a cross-platform ContentView or Layout), then measurement
			// has already happened via SizeThatFits and doesn't need to be repeated in LayoutSubviews. But we
			// _do_ need LayoutSubviews to make a measurement pass if the parent is something else (for example,
			// the window); there's no guarantee that SizeThatFits has been called in that case.

			if (!IsMeasureValid(widthConstraint, heightConstraint) && Superview is not MauiView)
			{
				CrossPlatformMeasure?.Invoke(widthConstraint, heightConstraint);
				CacheMeasureConstraints(widthConstraint, heightConstraint);
			}

			CrossPlatformArrange?.Invoke(bounds);

			if (ChildMaskLayer != null)
				ChildMaskLayer.Frame = bounds;

			SetClip();

			LayoutSubviewsChanged?.Invoke(this, EventArgs.Empty);
		}

		public override void SetNeedsLayout()
		{
			InvalidateConstraintsCache();
			base.SetNeedsLayout();
			Superview?.SetNeedsLayout();
		}

		internal Func<double, double, Size>? CrossPlatformMeasure { get; set; }
		internal Func<Rect, Size>? CrossPlatformArrange { get; set; }

		internal IBorderStroke? Clip
		{
			get
			{
				if (_clip?.TryGetTarget(out IBorderStroke? target) == true)
					return target;

				return null;
			}
			set
			{
				_clip = null;

				if (value != null)
					_clip = new WeakReference<IBorderStroke>(value);

				SetClip();
			}
		}

		CAShapeLayer? ChildMaskLayer
		{
			get => _childMaskLayer;
			set
			{
				var layer = GetChildLayer();

				if (layer != null && _childMaskLayer != null)
					layer.Mask = null;

				_childMaskLayer = value;

				if (layer != null)
					layer.Mask = value;
			}
		}

		CALayer? GetChildLayer()
		{
			if (Subviews.Length == 0)
				return null;

			var child = Subviews[0];

			if (child.Layer == null || child.Layer.Sublayers == null)
				return null;

			return child.Layer;
		}

		void SetClip()
		{
			if (Subviews.Length == 0)
				return;

			var mask = ChildMaskLayer;

			if (mask == null && Clip == null)
				return;

			mask ??= ChildMaskLayer = new CAShapeLayer();

			var frame = Frame;

			var bounds = new RectF(0, 0, (float)frame.Width, (float)frame.Height);

			IShape? clipShape = Clip?.Shape;
			var path = clipShape?.PathForBounds(bounds);
			var nativePath = path?.AsCGPath();
			mask.Path = nativePath;
		}
	}
}
﻿#nullable enable
using System;
using NativeView = Microsoft.UI.Xaml.FrameworkElement;

namespace Microsoft.Maui.Handlers
{
	public partial class ViewHandler
	{
		partial void ConnectingHandler(NativeView? nativeView)
		{
			if (nativeView != null)
			{
				nativeView.GotFocus += NativeViewGotFocus;
				nativeView.LostFocus += NativeViewLostFocus;
			}
		}

		partial void DisconnectingHandler(NativeView nativeView)
		{
			nativeView.GotFocus -= NativeViewGotFocus;
			nativeView.LostFocus -= NativeViewLostFocus;
		}
		
		static partial void MappingFrame(IViewHandler handler, IView view)
		{
			// Both Clip and Shadow depend on the Control size.
			handler.ToPlatform().UpdateClip(view);
			handler.ToPlatform().UpdateShadow(view);
		}

		public static void MapTranslationX(IViewHandler handler, IView view) 
		{ 
			handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapTranslationY(IViewHandler handler, IView view) 
		{ 
			handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapScale(IViewHandler handler, IView view) 
		{ 
			handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapScaleX(IViewHandler handler, IView view)
		{
			handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapScaleY(IViewHandler handler, IView view)
		{
			handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapRotation(IViewHandler handler, IView view) 
		{ 
			handler.ToPlatform().UpdateTransformation(view); 
		}

		public static void MapRotationX(IViewHandler handler, IView view) 
		{ 
			handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapRotationY(IViewHandler handler, IView view) 
		{ 
			handler.ToPlatform().UpdateTransformation(view); 
		}

		public static void MapAnchorX(IViewHandler handler, IView view) 
		{ 
			handler.ToPlatform().UpdateTransformation(view); 
		}

		public static void MapAnchorY(IViewHandler handler, IView view) 
		{ 
			handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapToolbar(IViewHandler handler, IView view)
		{
			if (view is IToolbarElement tb)
				MapToolbar(handler, tb);
		}

		internal static void MapToolbar(IElementHandler handler, IToolbarElement toolbarElement)
		{
			_ = handler.MauiContext ?? throw new InvalidOperationException($"{nameof(handler.MauiContext)} null");

			if (toolbarElement.Toolbar != null)
			{
				var toolBar = toolbarElement.Toolbar.ToPlatform(handler.MauiContext);
				handler.MauiContext.GetNavigationRootManager().SetToolbar(toolBar);
			}
		}

		void NativeViewGotFocus(object sender, UI.Xaml.RoutedEventArgs e)
		{
			if (VirtualView != null)
				VirtualView.IsFocused = true;
		}

		void NativeViewLostFocus(object sender, UI.Xaml.RoutedEventArgs e)
		{
			if (VirtualView != null)
				VirtualView.IsFocused = false;
		}
	}
}
﻿using System;
namespace Microsoft.Maui.Controls;

/// <summary>
/// Platform-specific arguments associated with the PointerEventArgs
/// </summary>
public class PlatformPointerEventArgs
{
#if IOS || MACCATALYST
	/// <summary>
	/// UIView that has the attached PointerGestureRecognizer
	/// </summary>
	public UIKit.UIView Sender { get; }

	/// <summary>
	/// UIGestureRecognizer attached to the UIView
	/// </summary>
	public UIKit.UIGestureRecognizer GestureRecognizer { get; }

	internal PlatformPointerEventArgs(UIKit.UIView sender, UIKit.UIGestureRecognizer gestureRecognizer)
	{
		Sender = sender;
		GestureRecognizer = gestureRecognizer;
	}

#elif ANDROID
	/// <summary>
	/// View that has the attached MotionEvent
	/// </summary>
	public Android.Views.View Sender { get; }

	/// <summary>
	/// MotionEvent attached to the View
	/// </summary>
	public Android.Views.MotionEvent MotionEvent { get; }

	internal PlatformPointerEventArgs(Android.Views.View sender, Android.Views.MotionEvent motionEvent)
	{
		Sender = sender;
		MotionEvent = motionEvent;
	}

#elif WINDOWS
	/// <summary>
	/// FrameworkElement that has the attached RoutedEventArgs
	/// </summary>
	public Microsoft.UI.Xaml.FrameworkElement Sender { get; }

	/// <summary>
	/// RoutedEventArgs attached to the FrameworkElement
	/// </summary>
	public Microsoft.UI.Xaml.RoutedEventArgs RoutedEventArgs { get; }

	internal PlatformPointerEventArgs(Microsoft.UI.Xaml.FrameworkElement sender, Microsoft.UI.Xaml.RoutedEventArgs routedEventArgs)
	{
		Sender = sender;
		RoutedEventArgs = routedEventArgs;
	}

#else
	internal PlatformPointerEventArgs()
	{
	}
#endif
}

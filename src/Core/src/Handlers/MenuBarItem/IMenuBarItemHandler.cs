﻿#if IOS || MACCATALYST
using PlatformView = UIKit.UIMenu;
#elif MONOANDROID
using PlatformView = Android.Views.View;
#elif WINDOWS
using PlatformView = Microsoft.UI.Xaml.Controls.MenuBarItem;
#elif NETSTANDARD || (NET6_0 && !IOS && !ANDROID)
using PlatformView = System.Object;
#endif

namespace Microsoft.Maui.Handlers
{
	public interface IMenuBarItemHandler : IElementHandler
	{
		void Add(IMenuFlyoutItemBase view);
		void Remove(IMenuFlyoutItemBase view);
		void Clear();
		void Insert(int index, IMenuFlyoutItemBase view);
		new PlatformView NativeView { get; }
		new IMenuBarItem VirtualView { get; }
	}
}

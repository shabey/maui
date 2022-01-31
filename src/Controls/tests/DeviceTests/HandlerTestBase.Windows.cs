﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation.Peers;
using NativeAutomationProperties = Microsoft.UI.Xaml.Automation.AutomationProperties;
using WPanel = Microsoft.UI.Xaml.Controls.Panel;
using WFrameworkElement = Microsoft.UI.Xaml.FrameworkElement;
using WWindow = Microsoft.UI.Xaml.Window;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Handlers;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Handlers;
using Microsoft.Maui.Platform;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Maui.DeviceTests
{
	public partial class HandlerTestBase
	{
		protected bool GetIsAccessibilityElement(IViewHandler viewHandler) =>
			((AccessibilityView)((DependencyObject)viewHandler.NativeView).GetValue(NativeAutomationProperties.AccessibilityViewProperty))
			== AccessibilityView.Content;

		Task RunWindowTest<THandler>(IWindow window, Func<THandler, Task> action)
			where THandler : class, IElementHandler
		{
			return InvokeOnMainThreadAsync(async () =>
			{
				var testingRootPanel = (WPanel)MauiProgram.CurrentWindow.Content;
				IElementHandler newWindowHandler = null;
				NavigationRootManager navigationRootManager = null;
				try
				{
					var scopedContext = new MauiContext(MauiContext.Services);
					scopedContext.AddWeakSpecific(MauiProgram.CurrentWindow);
					var mauiContext = scopedContext.MakeScoped(true);

					newWindowHandler = window.ToHandler(mauiContext);
					var content = window.Content.Handler.GetWrappedNativeView();
					navigationRootManager = mauiContext.GetNavigationRootManager();
					await content.LoadedAsync();
					await Task.Delay(10);

					if (typeof(THandler).IsAssignableFrom(newWindowHandler.GetType()))
						await action((THandler)newWindowHandler);
					else if (typeof(THandler).IsAssignableFrom(window.Content.Handler.GetType()))
						await action((THandler)window.Content.Handler);

				}
				finally
				{
					if (navigationRootManager != null)
						navigationRootManager.Disconnect();

					if (newWindowHandler != null)
						newWindowHandler.DisconnectHandler();

					// Set the root window panel back to the testing panel
					if (testingRootPanel != null && MauiProgram.CurrentWindow.Content != testingRootPanel)
					{
						MauiProgram.CurrentWindow.Content = testingRootPanel;
						await testingRootPanel.LoadedAsync();
						await Task.Delay(10);
					}
				}
			});
		}

		MauiNavigationView GetMauiNavigationView(NavigationRootManager navigationRootManager)
		{
			return (navigationRootManager.RootView as WindowRootView).NavigationViewControl;
		}

		protected MauiNavigationView GetMauiNavigationView(IMauiContext mauiContext)
		{
			return GetMauiNavigationView(mauiContext.GetNavigationRootManager());
		}

		protected Task CreateHandlerAndAddToWindow<THandler>(IElement view, Func<THandler, Task> action)
			where THandler : class, IElementHandler
		{
			return InvokeOnMainThreadAsync(async () =>
			{
				IWindow window = null;

				if (view is IWindow w)
				{
					window = w;
				}
				else if (view is Page page)
				{
					window = new Controls.Window(page);
				}
				else
				{
					window = new Controls.Window(new ContentPage() { Content = (View)view });
				}

				await RunWindowTest<THandler>(window, (handler) => action(handler as THandler));

				//WFrameworkElement frameworkElement = null;
				//var content = (WPanel)MauiContext.Services.GetService<WWindow>().Content;
				//THandler handler = null;
				//NavigationRootManager navigationRootManager = null;
				//try
				//{
				//	var mauiContext = MauiContext.MakeScoped(true);
				//	navigationRootManager = mauiContext
				//		.GetNavigationRootManager();

				//	navigationRootManager.Connect((IView)view);

				//	handler = (THandler)view.Handler;
				//	frameworkElement = (WFrameworkElement)handler.NativeView;
				//	content.Children.Add(navigationRootManager.RootView);
				//	await frameworkElement.LoadedAsync();
				//	await Task.Delay(10);
				//	await action(handler);
				//}
				//finally
				//{
				//	handler?.DisconnectHandler();
				//	navigationRootManager?.Disconnect();

				//	if (frameworkElement != null)
				//	{
				//		content.Children.Remove(navigationRootManager.RootView);
				//		await frameworkElement.UnloadedAsync();
				//		await Task.Delay(10);
				//	}
				//}
			});
		}
	}
}

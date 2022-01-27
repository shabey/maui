﻿#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WFrame = Microsoft.UI.Xaml.Controls.Frame;
using WApp = Microsoft.UI.Xaml.Application;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Media.Animation;
using WContentPresenter = Microsoft.UI.Xaml.Controls.ContentPresenter;
using WPage = Microsoft.UI.Xaml.Controls.Page;
using WThickness = Microsoft.UI.Xaml.Thickness;

namespace Microsoft.Maui.Controls.Handlers
{
	public partial class TabbedPageHandler : ViewHandler<TabbedPage, FrameworkElement>
	{
		MauiNavigationView? _navigationView;
		NavigationRootManager? _navigationRootManager;
		TabbedPage? _previousView;
		WFrame? _navigationFrame;
		WFrame NavigationFrame => _navigationFrame ?? throw new ArgumentNullException(nameof(NavigationFrame));

		protected override FrameworkElement CreateNativeView()
		{
			_navigationFrame = new WFrame();
			if (VirtualView.FindParentOfType<FlyoutPage>() != null)
			{
				_navigationView = new MauiNavigationView()
				{
					Content = _navigationFrame,
					PaneDisplayMode = NavigationViewPaneDisplayMode.LeftMinimal,
					IsBackButtonVisible = NavigationViewBackButtonVisible.Collapsed,
					IsSettingsVisible = false,
					IsPaneToggleButtonVisible = false
				};

				_navigationView.OnApplyTemplateFinished += OnApplyTemplateFinished;

				// Unset styles set by parent NavigationView
				_navigationView.UpdateResourceToApplicationDefault("NavigationViewContentMargin", null);
				_navigationView.UpdateResourceToApplicationDefault("NavigationViewMinimalHeaderMargin", null);
				_navigationView.UpdateResourceToApplicationDefault("NavigationViewHeaderMargin", null);
				_navigationView.UpdateResourceToApplicationDefault("NavigationViewMinimalContentGridBorderThickness", null);

				SetupNavigationView();
				return _navigationView;
			}

			_navigationRootManager = MauiContext?.GetNavigationRootManager();
			return _navigationFrame;
		}

		private protected override void OnConnectHandler(FrameworkElement nativeView)
		{
			base.OnConnectHandler(nativeView);
			NavigationFrame.Navigated += OnNavigated;

			// If CreateNativeView didn't set the NavigationView then that means we are using the
			// WindowRootView for our tabs
			if (_navigationView == null)
			{
				_navigationRootManager = MauiContext?.GetNavigationRootManager();
				_navigationView = (_navigationRootManager?.RootView as WindowRootView)?.NavigationViewControl;
				SetupNavigationView();
			}
		}

		private protected override void OnDisconnectHandler(FrameworkElement nativeView)
		{
			if (_navigationView != null)
				_navigationView.OnApplyTemplateFinished -= OnApplyTemplateFinished;

			((WFrame)nativeView).Navigated -= OnNavigated;
			VirtualView.Appearing -= OnTabbedPageAppearing;
			VirtualView.Disappearing -= OnTabbedPageDisappearing;
			if (_navigationView != null)
				_navigationView.SelectionChanged -= OnSelectedMenuItemChanged;

			_navigationView = null;
			_navigationRootManager = null;
			_previousView = null;
			_navigationFrame = null;

			base.OnDisconnectHandler(nativeView);
		}

		public override void SetVirtualView(IView view)
		{
			if (_previousView != null)
			{
				_previousView.Appearing -= OnTabbedPageAppearing;
				_previousView.Disappearing -= OnTabbedPageDisappearing;
			}

			base.SetVirtualView(view);

			_previousView = VirtualView;
			VirtualView.Appearing += OnTabbedPageAppearing;
			VirtualView.Disappearing += OnTabbedPageDisappearing;
		}

		void OnTabbedPageAppearing(object? sender, EventArgs e)
		{
			if (_navigationView != null)
				_navigationView.PaneDisplayMode = NavigationViewPaneDisplayMode.Top;
		}

		void OnTabbedPageDisappearing(object? sender, EventArgs e)
		{
			if (_navigationView != null)
				_navigationView.PaneDisplayMode = NavigationViewPaneDisplayMode.LeftMinimal;
		}

		void OnApplyTemplateFinished(object? sender, EventArgs e)
		{
			UpdateValuesWaitingForNavigationView();
		}

		void SetupNavigationView()
		{
			if (_navigationView == null)
				return;

			_navigationView.PaneDisplayMode = NavigationViewPaneDisplayMode.Top;
			_navigationView.MenuItemTemplate = (UI.Xaml.DataTemplate)WApp.Current.Resources["TabBarNavigationViewMenuItem"];

			if (_navigationView.TopNavArea != null)
				UpdateValuesWaitingForNavigationView();
			else
				_navigationView.OnApplyTemplateFinished += OnApplyTemplateFinished;
		}

		void UpdateValuesWaitingForNavigationView()
		{
			if (_navigationView == null)
				return;

			UpdateValue(nameof(TabbedPage.BarBackground));
			UpdateValue(nameof(TabbedPage.ItemsSource));

			_navigationView.SelectionChanged += OnSelectedMenuItemChanged;
			_navigationView.SelectedItem = VirtualView.CurrentPage;
		}

		void OnSelectedMenuItemChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
		{
			if (args.SelectedItem is Page page)
				NavigateToPage(page);
		}

		void NavigateToPage(Page page)
		{
			FrameNavigationOptions navOptions = new FrameNavigationOptions();
			VirtualView.CurrentPage = page;
			navOptions.IsNavigationStackEnabled = false;
			NavigationFrame.NavigateToType(typeof(WPage), null, navOptions);
		}


		void UpdateCurrentPageContent()
		{
			if (NavigationFrame.Content is WPage page)
				UpdateCurrentPageContent(page);
		}

		void UpdateCurrentPageContent(WPage page)
		{
			if (MauiContext == null)
				return;

			WContentPresenter? presenter;
			IView _currentPage = VirtualView.CurrentPage;

			if (page.Content == null)
			{
				presenter = new WContentPresenter()
				{
					HorizontalAlignment = UI.Xaml.HorizontalAlignment.Stretch,
					VerticalAlignment = UI.Xaml.VerticalAlignment.Stretch
				};

				page.Content = presenter;
			}
			else
			{
				presenter = page.Content as WContentPresenter;
			}

			// At this point if the Content isn't a ContentPresenter the user has replaced
			// the conent so we just let them take control
			if (presenter == null || _currentPage == null)
				return;

			presenter.Content = _currentPage.ToNative(MauiContext);
		}

		void OnNavigated(object sender, UI.Xaml.Navigation.NavigationEventArgs e)
		{
			if (e.Content is WPage page)
				UpdateCurrentPageContent(page);
		}

		public static void MapBarBackground(TabbedPageHandler handler, TabbedPage view)
		{
			handler._navigationView?.UpdateTopNavAreaBackground(view.BarBackground ?? view.BarBackgroundColor?.AsPaint());
		}

		public static void MapBarBackgroundColor(TabbedPageHandler handler, TabbedPage view)
		{
			MapBarBackground(handler, view);
		}

		public static void MapBarTextColor(TabbedPageHandler handler, TabbedPage view)
		{
		}

		public static void MapUnselectedTabColor(TabbedPageHandler handler, TabbedPage view)
		{
		}

		public static void MapSelectedTabColor(TabbedPageHandler handler, TabbedPage view)
		{
		}

		public static void MapItemsSource(TabbedPageHandler handler, TabbedPage view)
		{
			if (handler._navigationView != null)
				handler._navigationView.MenuItemsSource = handler.VirtualView.Children;
		}

		public static void MapItemTemplate(TabbedPageHandler handler, TabbedPage view)
		{
			handler.UpdateCurrentPageContent();
		}

		public static void MapSelectedItem(TabbedPageHandler handler, TabbedPage view)
		{
			handler.UpdateCurrentPageContent();
		}

		public static void MapCurrentPage(TabbedPageHandler handler, TabbedPage view)
		{
			if (handler._navigationView != null && handler._navigationView.SelectedItem != view.CurrentPage)
				handler._navigationView.SelectedItem = view.CurrentPage;
		}
	}
}

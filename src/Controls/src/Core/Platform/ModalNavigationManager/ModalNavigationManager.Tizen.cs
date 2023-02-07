﻿#nullable enable

using System.Threading.Tasks;
using Tizen.UIExtensions.NUI;

namespace Microsoft.Maui.Controls.Platform
{
	internal partial class ModalNavigationManager
	{
		NavigationStack _modalStack => WindowMauiContext.GetModalStack();
		IPageController CurrentPageController => _navModel.CurrentPage;

		partial void OnPageAttachedHandler()
		{
			WindowMauiContext.GetPlatformWindow().SetBackButtonPressedHandler(OnBackButtonPressed);
		}

		async Task<Page> PopModalPlatformAsync(bool animated)
		{
			Page modal = CurrentPlatformModalPage;
			_platformModalPages.Remove(modal);

			((IPageController)modal).SendDisappearing();

			var modalRenderer = modal.Handler as IPlatformViewHandler;
			if (modalRenderer != null)
			{
				await _modalStack.Pop(animated);
				CurrentPageController?.SendAppearing();
				(modal.Handler as IPlatformViewHandler)?.Dispose();
			}
			return modal;
		}

		async Task PushModalPlatformAsync(Page modal, bool animated)
		{
			CurrentPageController?.SendDisappearing();
			_platformModalPages.Add(modal);

			var nativeView = modal.ToPlatform(WindowMauiContext);

			await _modalStack.Push(nativeView, animated);

			// Verify that the modal is still on the stack
			if (_navModel.CurrentPage == modal)
				((IPageController)modal).SendAppearing();
		}

		bool OnBackButtonPressed()
		{
			Page root = _navModel.LastRoot;
			bool handled = root?.SendBackButtonPressed() ?? false;

			return handled;
		}
	}
}

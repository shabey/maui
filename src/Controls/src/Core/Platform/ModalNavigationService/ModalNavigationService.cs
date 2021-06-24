﻿#nullable enable

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Internals;

namespace Microsoft.Maui.Controls.Platform
{
	internal partial class ModalNavigationService
	{
		Window _window;
		public IReadOnlyList<Page> ModalStack => _navModel.Modals;
		IMauiContext MauiContext => _window.MauiContext;
		NavigationModel _navModel = new NavigationModel();
		NavigationModel? _previousNavModel = null;
		Page? _previousPage;

		public ModalNavigationService(Window window)
		{
			_window = window;
		}

		public Task<Page> PopModalAsync()
		{
			return PopModalAsync(true);
		}


		public Task PushModalAsync(Page modal)
		{
			return PushModalAsync(modal, true);
		}

		internal void SettingNewPage()
		{
			if (_previousPage != null)
			{
				_previousPage.AttachedHandler -= AttachedHandler;

				// if _previousNavModel has been set than _navModel has already been reinitialized
				if (_previousNavModel != null)
				{
					_previousNavModel = null;
					if (_navModel == null)
						_navModel = new NavigationModel();
				}
				else
					_navModel = new NavigationModel();
			}

			if (_window.Page == null)
			{
				_previousPage = null;

				return;
			}

			_navModel.Push(_window.Page, null);
			_previousPage = _window.Page;
			_previousPage.AttachedHandler += AttachedHandler;
			_previousPage.DetachedHandler += DetachedHandler;
		}

		partial void OnAttachedHandler();
		void AttachedHandler(object? sender, EventArgs e) => OnAttachedHandler();

		partial void OnDetachedHandler();
		void DetachedHandler(object? sender, EventArgs e) => OnDetachedHandler();
	}
}

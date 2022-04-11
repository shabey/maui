﻿using System;
#if WEBVIEW2_WINFORMS
using Microsoft.AspNetCore.Components.WebView.WindowsForms;
#elif WEBVIEW2_WPF
using Microsoft.AspNetCore.Components.WebView.Wpf;
#elif WEBVIEW2_MAUI
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Maui.Hosting;
#else
#error Must define WEBVIEW2_WINFORMS, WEBVIEW2_WPF, WEBVIEW2_MAUI
#endif
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	/// Extension methods to <see cref="IServiceCollection"/>.
	/// </summary>
	public static class BlazorWebViewServiceCollectionExtensions
	{
		/// <summary>
		/// Configures <see cref="IServiceCollection"/> to add support for <see cref="BlazorWebView"/>.
		/// </summary>
		/// <param name="services">The <see cref="IServiceCollection"/>.</param>
		/// <returns>The <see cref="IServiceCollection"/>.</returns>
#if WEBVIEW2_WINFORMS
		public static IServiceCollection AddWindowsFormsBlazorWebView(this IServiceCollection services)
#elif WEBVIEW2_WPF
		public static IServiceCollection AddWpfBlazorWebView(this IServiceCollection services)
#elif WEBVIEW2_MAUI
		public static IMauiBlazorWebViewBuilder AddMauiBlazorWebView(this IServiceCollection services)
#else
#error Must define WEBVIEW2_WINFORMS, WEBVIEW2_WPF, WEBVIEW2_MAUI
#endif
		{
			services.AddBlazorWebView();
			services.TryAddSingleton(new BlazorWebViewDeveloperTools { Enabled = false });
#if WEBVIEW2_MAUI
			services.TryAddSingleton<MauiBlazorMarkerService>();
			services.ConfigureMauiHandlers(static handlers => handlers.AddHandler<IBlazorWebView, BlazorWebViewHandler>());
#elif WEBVIEW2_WINFORMS
			services.TryAddSingleton<WindowsFormsBlazorMarkerService>();
#elif WEBVIEW2_WPF
			services.TryAddSingleton<WpfBlazorMarkerService>();
#endif

#if WEBVIEW2_MAUI
			return new MauiBlazorWebViewBuilder();
#elif WEBVIEW2_WINFORMS
			return services;
#elif WEBVIEW2_WPF
			return services;
#endif
		}

		/// <summary>
		/// Enables Developer tools on the underlying WebView controls.
		/// </summary>
		/// <param name="services">The <see cref="IServiceCollection"/>.</param>
		/// <returns>The <see cref="IServiceCollection"/>.</returns>
#if WEBVIEW2_WINFORMS
		public static IServiceCollection AddBlazorWebViewDeveloperTools(this IServiceCollection services)
#elif WEBVIEW2_WPF
		public static IServiceCollection AddBlazorWebViewDeveloperTools(this IServiceCollection services)
#elif WEBVIEW2_MAUI
		public static IMauiBlazorWebViewBuilder AddBlazorWebViewDeveloperTools(this IServiceCollection services)
#else
#error Must define WEBVIEW2_WINFORMS, WEBVIEW2_WPF, WEBVIEW2_MAUI
#endif
		{
			services.AddSingleton<BlazorWebViewDeveloperTools>(new BlazorWebViewDeveloperTools { Enabled = true });

#if WEBVIEW2_MAUI
			return new MauiBlazorWebViewBuilder();
#elif WEBVIEW2_WINFORMS
			return services;
#elif WEBVIEW2_WPF
			return services;
#endif
		}
	}
}

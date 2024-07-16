﻿using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using Windows.Storage.Streams;

namespace Microsoft.Maui.Handlers
{
	public partial class HybridWebViewHandler : ViewHandler<IHybridWebView, WebView2>
	{
		private readonly HybridWebView2Proxy _proxy = new();

		protected override WebView2 CreatePlatformView()
		{
			return new MauiHybridWebView(this);
		}

		protected override async void ConnectHandler(WebView2 platformView)
		{
			_proxy.Connect(this, platformView);

			base.ConnectHandler(platformView);

			if (platformView.IsLoaded)
			{
				OnLoaded();
			}
			else
			{
				platformView.Loaded += OnWebViewLoaded;
			}

			await platformView.EnsureCoreWebView2Async();

			platformView.CoreWebView2.Settings.AreDevToolsEnabled = true;//EnableWebDevTools;
			platformView.CoreWebView2.Settings.IsWebMessageEnabled = true;
			platformView.CoreWebView2.AddWebResourceRequestedFilter($"{AppOrigin}*", CoreWebView2WebResourceContext.All);
			platformView.CoreWebView2.WebResourceRequested += CoreWebView2_WebResourceRequested;

			platformView.Source = new Uri(new Uri(AppOriginUri, "/").ToString());
		}

		void OnWebViewLoaded(object sender, RoutedEventArgs e)
		{
			OnLoaded();
		}

		void OnLoaded()
		{
			var window = MauiContext!.GetPlatformWindow();
			_proxy.Connect(window);
		}

		void Disconnect(WebView2 platformView)
		{
			platformView.Loaded -= OnWebViewLoaded;
			_proxy.Disconnect(platformView);
			if (platformView.CoreWebView2 is not null)
			{
				platformView.Close();
			}
		}

		protected override void DisconnectHandler(WebView2 platformView)
		{
			Disconnect(platformView);
			base.DisconnectHandler(platformView);
		}

		public static void MapSendRawMessage(IHybridWebViewHandler handler, IHybridWebView hybridWebView, object? arg)
		{
			if (arg is not string rawMessage || handler.PlatformView is not IHybridPlatformWebView hybridPlatformWebView)
			{
				return;
			}

			hybridPlatformWebView.SendRawMessage(rawMessage);
		}


		private void OnWebMessageReceived(WebView2 sender, CoreWebView2WebMessageReceivedEventArgs args)
		{
			VirtualView?.RawMessageReceived(args.TryGetWebMessageAsString());
		}

		private async void CoreWebView2_WebResourceRequested(CoreWebView2 sender, CoreWebView2WebResourceRequestedEventArgs eventArgs)
		{
			// Get a deferral object so that WebView2 knows there's some async stuff going on. We call Complete() at the end of this method.
			using var deferral = eventArgs.GetDeferral();

			var requestUri = HybridWebViewQueryStringHelper.RemovePossibleQueryString(eventArgs.Request.Uri);

			if (new Uri(requestUri) is Uri uri && AppOriginUri.IsBaseOf(uri))
			{
				var relativePath = AppOriginUri.MakeRelativeUri(uri).ToString().Replace('/', '\\');

				string contentType;
				if (string.IsNullOrEmpty(relativePath))
				{
					relativePath = VirtualView.DefaultFile;
					contentType = "text/html";
				}
				else
				{
					if (!ContentTypeProvider.TryGetContentType(relativePath, out contentType!))
					{
						// TODO: Log this
						contentType = "text/plain";
					}
				}

				var assetPath = Path.Combine(VirtualView.HybridRoot!, relativePath!);
				var contentStream = await GetAssetStreamAsync(assetPath);

				if (contentStream is null)
				{
					var notFoundContent = "Resource not found (404)";
					eventArgs.Response = sender.Environment!.CreateWebResourceResponse(
						Content: null,
						StatusCode: 404,
						ReasonPhrase: "Not Found",
						Headers: GetHeaderString("text/plain", notFoundContent.Length)
					);
				}
				else
				{
					eventArgs.Response = sender.Environment!.CreateWebResourceResponse(
						Content: await CopyContentToRandomAccessStreamAsync(contentStream),
						StatusCode: 200,
						ReasonPhrase: "OK",
						Headers: GetHeaderString(contentType, (int)contentStream.Length)
					);
				}

				contentStream?.Dispose();
			}

			// Notify WebView2 that the deferred (async) operation is complete and we set a response.
			deferral.Complete();

			async Task<IRandomAccessStream> CopyContentToRandomAccessStreamAsync(Stream content)
			{
				using var memStream = new MemoryStream();
				await content.CopyToAsync(memStream);
				var randomAccessStream = new InMemoryRandomAccessStream();
				await randomAccessStream.WriteAsync(memStream.GetWindowsRuntimeBuffer());
				return randomAccessStream;
			}
		}

		private protected static string GetHeaderString(string contentType, int contentLength) =>
$@"Content-Type: {contentType}
Content-Length: {contentLength}";

		private sealed class HybridWebView2Proxy
		{
			private WeakReference<Window>? _window;
			private WeakReference<HybridWebViewHandler>? _handler;

			private Window? Window => _window is not null && _window.TryGetTarget(out var w) ? w : null;
			private HybridWebViewHandler? Handler => _handler is not null && _handler.TryGetTarget(out var h) ? h : null;

			public void Connect(HybridWebViewHandler handler, WebView2 platformView)
			{
				_handler = new(handler);

				platformView.WebMessageReceived += OnWebMessageReceived;
			}

			public void Connect(Window window)
			{
				_window = new(window);
				window.Closed += OnWindowClosed;
			}

			public void Disconnect(WebView2 platformView)
			{
				platformView.WebMessageReceived -= OnWebMessageReceived;

				if (platformView.CoreWebView2 is CoreWebView2 webView2)
				{
					webView2.Stop();
				}

				if (Window is Window window)
				{
					window.Closed -= OnWindowClosed;
				}

				_handler = null;
				_window = null;
			}

			void OnWebMessageReceived(WebView2 sender, CoreWebView2WebMessageReceivedEventArgs args)
			{
				if (Handler is HybridWebViewHandler handler)
				{
					handler.OnWebMessageReceived(sender, args);
				}
			}

			void OnWindowClosed(object sender, WindowEventArgs args)
			{
				if (Handler is HybridWebViewHandler handler)
				{
					handler.Disconnect(handler.PlatformView);
				}
			}
		}
	}
}

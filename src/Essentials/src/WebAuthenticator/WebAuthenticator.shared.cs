using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Microsoft.Maui.Authentication;

namespace Microsoft.Maui.Authentication
{
	public interface IWebAuthenticator
	{
		Task<WebAuthenticatorResult> AuthenticateAsync(Uri url, Uri callbackUrl);

		Task<WebAuthenticatorResult> AuthenticateAsync(WebAuthenticatorOptions webAuthenticatorOptions);
	}

	public interface IPlatformWebAuthenticatorCallback
	{
#if IOS || MACCATALYST || MACOS
		bool OpenUrlCallback(Uri uri);
#elif ANDROID
		bool OnResumeCallback(global::Android.Content.Intent intent);
#endif
	}
}

namespace Microsoft.Maui.Essentials
{
	/// <include file="../../docs/Microsoft.Maui.Essentials/WebAuthenticator.xml" path="Type[@FullName='Microsoft.Maui.Essentials.WebAuthenticator']/Docs" />
	public static partial class WebAuthenticator
	{
		/// <include file="../../docs/Microsoft.Maui.Essentials/WebAuthenticator.xml" path="//Member[@MemberName='AuthenticateAsync'][1]/Docs" />
		public static Task<WebAuthenticatorResult> AuthenticateAsync(Uri url, Uri callbackUrl)
			=> Current.AuthenticateAsync(url, callbackUrl);

		/// <include file="../../docs/Microsoft.Maui.Essentials/WebAuthenticator.xml" path="//Member[@MemberName='AuthenticateAsync'][2]/Docs" />
		public static Task<WebAuthenticatorResult> AuthenticateAsync(WebAuthenticatorOptions webAuthenticatorOptions)
			=> Current.AuthenticateAsync(webAuthenticatorOptions);

#if IOS || MACCATALYST || MACOS
		internal static bool OpenUrl(Uri uri)
		{
			if (Current is IPlatformWebAuthenticatorCallback c)
				return c.OpenUrlCallback(uri);
			return false;
		}
#elif ANDROID
		internal static bool OnResume(global::Android.Content.Intent intent)
		{
			if (Current is IPlatformWebAuthenticatorCallback c)
				return c.OnResumeCallback(intent);
			return false;
		}
#endif

#nullable enable
		static IWebAuthenticator? currentImplementation;

		public static IWebAuthenticator Current =>
			currentImplementation ??= new WebAuthenticatorImplementation();

		internal static void SetCurrent(IWebAuthenticator? implementation) =>
			currentImplementation = implementation;
#nullable disable
	}
}

namespace Microsoft.Maui.Authentication
{
	public class WebAuthenticatorOptions
	{
		public Uri Url { get; set; }

		public Uri CallbackUrl { get; set; }

		public bool PrefersEphemeralWebBrowserSession { get; set; }
	}

	public partial class WebAuthenticatorImplementation
	{
		public Task<WebAuthenticatorResult> AuthenticateAsync(Uri url, Uri callbackUrl)
			=> AuthenticateAsync(new WebAuthenticatorOptions { Url = url, CallbackUrl = callbackUrl });
	}
}

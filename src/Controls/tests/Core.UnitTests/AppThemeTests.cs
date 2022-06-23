using System;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Graphics;
using Xunit;

namespace Microsoft.Maui.Controls.Core.UnitTests
{
	public class AppThemeTests : BaseTestFixture
	{
		MockAppInfo mockAppInfo;
		Application app;

		public AppThemeTests()
		{
			AppInfo.SetCurrent(mockAppInfo = new MockAppInfo() { RequestedTheme = AppTheme.Light });
			Application.Current = app = new Application();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Application.Current = null;
			}

			base.Dispose(disposing);
		}

		[Fact]
		public void ThemeChangeUsingSetAppThemeColor()
		{
			var label = new Label
			{
				Text = "Green on Light, Red on Dark"
			};

			label.SetAppThemeColor(Label.TextColorProperty, Colors.Green, Colors.Red);
			Assert.Equal(Colors.Green, label.TextColor);

			SetAppTheme(AppTheme.Dark);

			Assert.Equal(Colors.Red, label.TextColor);
		}

		[Fact]
		public void ThemeChangeUsingSetAppTheme()
		{
			var label = new Label
			{
				Text = "Green on Light, Red on Dark"
			};

			label.SetAppTheme(Label.TextColorProperty, Colors.Green, Colors.Red);
			Assert.Equal(Colors.Green, label.TextColor);

			SetAppTheme(AppTheme.Dark);

			Assert.Equal(Colors.Red, label.TextColor);
		}

		[Fact]
		public void ThemeChangeUsingSetBinding()
		{
			var label = new Label
			{
				Text = "Green on Light, Red on Dark"
			};

			label.SetBinding(Label.TextColorProperty, new AppThemeBinding { Light = Colors.Green, Dark = Colors.Red });
			Assert.Equal(Colors.Green, label.TextColor);

			SetAppTheme(AppTheme.Dark);

			Assert.Equal(Colors.Red, label.TextColor);
		}

		[Fact]
		public void ThemeChangeUsingUserAppTheme()
		{
			var label = new Label
			{
				Text = "Green on Light, Red on Dark"
			};

			label.SetAppThemeColor(Label.TextColorProperty, Colors.Green, Colors.Red);
			Assert.Equal(Colors.Green, label.TextColor);

			app.UserAppTheme = AppTheme.Dark;

			Assert.Equal(Colors.Red, label.TextColor);
		}

		[Fact]
		public void InitialThemeIsCorrect()
		{
			var changed = 0;
			var newTheme = AppTheme.Unspecified;

			app.RequestedThemeChanged += (_, e) =>
			{
				changed++;
				newTheme = e.RequestedTheme;
			};

			Assert.Equal(AppTheme.Light, app.RequestedTheme);
			Assert.Equal(AppTheme.Light, app.PlatformAppTheme);
			Assert.Equal(AppTheme.Unspecified, app.UserAppTheme);

			Assert.Equal(0, changed);
			Assert.Equal(AppTheme.Unspecified, newTheme);
		}

		[Fact]
		public void SettingSameUserThemeDoesNotFireEvent()
		{
			var changed = 0;
			var newTheme = AppTheme.Unspecified;

			app.RequestedThemeChanged += (_, e) =>
			{
				changed++;
				newTheme = e.RequestedTheme;
			};

			app.UserAppTheme = AppTheme.Light;

			Assert.Equal(AppTheme.Light, app.RequestedTheme);
			Assert.Equal(AppTheme.Light, app.PlatformAppTheme);
			Assert.Equal(AppTheme.Light, app.UserAppTheme);

			Assert.Equal(0, changed);
			Assert.Equal(AppTheme.Unspecified, newTheme);
		}

		[Fact]
		public void SettingDifferentUserThemeDoesNotFireEvent()
		{
			var changed = 0;
			var newTheme = AppTheme.Unspecified;

			app.RequestedThemeChanged += (_, e) =>
			{
				changed++;
				newTheme = e.RequestedTheme;
			};

			app.UserAppTheme = AppTheme.Dark;

			Assert.Equal(AppTheme.Dark, app.RequestedTheme);
			Assert.Equal(AppTheme.Light, app.PlatformAppTheme);
			Assert.Equal(AppTheme.Dark, app.UserAppTheme);

			Assert.Equal(1, changed);
			Assert.Equal(AppTheme.Dark, newTheme);
		}

		[Fact]
		public void UnsettingUserThemeReverts()
		{
			var changed = 0;
			var newTheme = AppTheme.Unspecified;

			app.RequestedThemeChanged += (_, e) =>
			{
				changed++;
				newTheme = e.RequestedTheme;
			};

			app.UserAppTheme = AppTheme.Dark;
			app.UserAppTheme = AppTheme.Unspecified;

			Assert.Equal(AppTheme.Light, app.RequestedTheme);
			Assert.Equal(AppTheme.Light, app.PlatformAppTheme);
			Assert.Equal(AppTheme.Unspecified, app.UserAppTheme);

			Assert.Equal(2, changed);
			Assert.Equal(AppTheme.Light, newTheme);
		}

		void SetAppTheme(AppTheme theme)
		{
			mockAppInfo.RequestedTheme = theme;
			((IApplication)app).ThemeChanged();
		}

		[Fact]
		//https://github.com/dotnet/maui/issues/3188
		public void ThemeBindingRemovedOnOneTimeBindablePropertyWhenPropertySet()
		{
			var shell = new Shell();
			shell.SetAppThemeColor(Shell.FlyoutBackgroundProperty, Colors.White, Colors.Black);
			shell.FlyoutBackgroundColor = Colors.Pink;
			SetAppTheme(AppTheme.Dark);
			Assert.Equal(Colors.Pink, shell.FlyoutBackgroundColor);
		}

		[Test]
		public void NullApplicationCurrentFallsBackToEssentials()
		{
			var label = new Label
			{
				Text = "Green on Light, Red on Dark"
			};

			label.SetAppThemeColor(Label.TextColorProperty, Colors.Green, Colors.Red);

			Application.Current = null;

			Assert.AreEqual(Colors.Green, label.TextColor);

			SetAppTheme(AppTheme.Dark);

			Assert.AreEqual(Colors.Red, label.TextColor);
		}
	}
}
﻿using System.Reflection;
using Microsoft.Maui.Appium;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TestUtils.Appium.UITests;

namespace Microsoft.Maui.AppiumTests
{
#if ANDROID
	[TestFixture(TestDevice.Android)]
#elif IOSUITEST
	[TestFixture(TestDevice.iOS)]
#elif MACUITEST
	[TestFixture(TestDevice.Mac)]
#elif WINTEST
	[TestFixture(TestDevice.Windows)]
#else
	[TestFixture(TestDevice.iOS)]
	[TestFixture(TestDevice.Mac)]
	[TestFixture(TestDevice.Windows)]
	[TestFixture(TestDevice.Android)]
#endif
	public class UITestBase : UITestContextTestBase
	{
		readonly TestDevice _testDevice;
		public UITestBase(TestDevice device)
		{
			_testDevice = device;
		}

		protected virtual void FixtureSetup() { }

		protected virtual void FixtureTeardown() { }

		[TearDown]
		public void UITestBaseTearDown()
		{
			var testOutcome = TestContext.CurrentContext.Result.Outcome;
			if (testOutcome == ResultState.Error || testOutcome == ResultState.Failure)
				SaveScreenshotAndPageSource("UITestBaseTearDown");
		}

		[OneTimeSetUp]
		public void OneTimeSetup()
		{
			InitialSetup(TestContextSetupFixture.TestContext);
			try
			{
				FixtureSetup();
			}
			catch
			{
				SaveScreenshotAndPageSource("OneTimeSetup");
				throw;
			}
		}

		[OneTimeTearDown()]
		public void OneTimeTearDown()
		{
			var testOutcome = TestContext.CurrentContext.Result.Outcome;
			if (testOutcome == ResultState.Error || testOutcome == ResultState.Failure)
				SaveScreenshotAndPageSource("OneTimeTearDown");

			FixtureTeardown();
		}

		void SaveScreenshotAndPageSource(string note)
		{
			var logDir = (Path.GetDirectoryName(Environment.GetEnvironmentVariable("APPIUM_LOG_FILE")) ?? Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))!;

			_ = App.Screenshot(Path.Combine(logDir, $"{GetType().FullName}-{TestContext.CurrentContext.Test.MethodName}-{note}-{UITestContext.TestConfig.TestDevice}-ScreenShot"));

			if (App is IApp2 app2)
			{
				var pageSource = app2.ElementTree;
				File.WriteAllText(Path.Combine(logDir, $"{GetType().FullName}-{TestContext.CurrentContext.Test.MethodName}-{note}-{UITestContext.TestConfig.TestDevice}-PageSource.txt"), pageSource);
			}

			foreach (var log in Directory.GetFiles(logDir))
			{
				TestContext.AddTestAttachment(log, Path.GetFileName(log));
			}
		}

		public override TestConfig GetTestConfig()
		{
			var appProjectFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "..\\..\\..\\..\\..\\samples\\Controls.Sample.UITests");
			var appProjectPath = Path.Combine(appProjectFolder, "Controls.Sample.UITests.csproj");
			var testConfig = new TestConfig(_testDevice, "com.microsoft.maui.uitests")
			{
				BundleId = "com.microsoft.maui.uitests",
				AppProjectPath = appProjectPath
			};
			var windowsExe = "Controls.Sample.UITests.exe";
			var windowsExePath = Path.Combine(appProjectFolder, $"bin\\{testConfig.Configuration}\\{testConfig.FrameworkVersion}-windows10.0.20348\\win10-x64\\{windowsExe}");

			switch (_testDevice)
			{
				case TestDevice.iOS:
					testConfig.DeviceName = "iPhone X";
					testConfig.PlatformVersion = Environment.GetEnvironmentVariable("IOS_PLATFORM_VERSION") ?? "14.4";
					testConfig.Udid = Environment.GetEnvironmentVariable("IOS_SIMULATOR_UDID") ?? "";
					break;
				case TestDevice.Windows:
					testConfig.DeviceName = "WindowsPC";
					testConfig.AppPath = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WINDOWS_APP_PATH"))
						? windowsExePath
						: Environment.GetEnvironmentVariable("WINDOWS_APP_PATH");
					break;
			}

			return testConfig;
		}
	}
}

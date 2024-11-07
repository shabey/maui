﻿#if !MACCATALYST
using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues
{
    internal class Issue25436 : _IssuesUITest
	{
		public override string Issue => "[.NET 9] Shell Flyout menu not rendering after navigating from a MenuItem page";

		public Issue25436(TestDevice testDevice) : base(testDevice) { }

		[Test]
		[Category(UITestCategories.Shell)]
		public void FlyoutMenuShouldNotDisappearWhenNavigateUsingServices()
		{
			App.WaitForElement("BackButton");
			App.Tap("BackButton");
			App.WaitForElement("Login");
			App.Tap("Login");
#if ANDROID
			App.TapCoordinates(60,130);
#else
			App.Tap(FlyoutIconAutomationId);
#endif
			VerifyScreenshot();
		}
			
	}
}
#endif
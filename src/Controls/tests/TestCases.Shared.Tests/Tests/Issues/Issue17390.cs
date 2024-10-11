﻿#if !MACCATALYST
using NUnit.Framework;
using NUnit.Framework.Legacy;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues
{
	internal class Issue17390 : _IssuesUITest
	{
		public override string Issue => "Shell where the bottom padding is not calculated properly when navigating between the tabs";

		public Issue17390(TestDevice device) : base(device)
		{
		}

		[Test]
		[Category(UITestCategories.Shell)]
		public void ShellBottomPaddingWhenNavigatingBetweenTabs()
		{
			// Is a iOS issue; see https://github.com/dotnet/maui/issues/17390
			App.WaitForElement("MainTabPage");
			App.Click("InnerTabbedPageButton");
			App.Click("OpenNonTabbedPage");
			App.Click("BackToTabbedPageButton");
			VerifyScreenshot();
		}
	}
}
#endif

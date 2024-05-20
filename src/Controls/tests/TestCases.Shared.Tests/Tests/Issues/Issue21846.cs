﻿using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues
{
	public class Issue21846 : _IssuesUITest
	{
		public Issue21846(TestDevice device) : base(device)
		{
		}

		public override string Issue => "Fix crash closing Popup with WebView";

		[Test]
		[Category(UITestCategories.WebView)]
		[FailsOnAndroid]
		[FailsOnWindows]
		public void WebViewNoCrashPopup()
		{
			App.WaitForElement("OpenModalButton");
			App.Click("OpenModalButton");

			App.WaitForElement("CloseModalButton");
			App.Click("CloseModalButton");
		}
	}
}

﻿#if !MACCATALYST
using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Tests.Issues
{
	public class Issue22937 : _IssuesUITest
	{
		public override string Issue => "[Android] ToolbarItem font color not updating properly after changing the available state at runtime";

		public Issue22937(TestDevice device)
		: base(device)
		{ }

		[Test]
		[Category(UITestCategories.ToolbarItem)]
		public async void ToolbarItemFontColorDynamicUpdate()
		{
			App.WaitForElement("ChangeState");
			App.Tap("ChangeState");

			await Task.Delay(100);

			VerifyScreenshot();
		}
	}
}
#endif
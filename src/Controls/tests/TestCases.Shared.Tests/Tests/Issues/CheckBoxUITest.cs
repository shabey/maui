﻿using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues
{
	public class CheckBoxUITest : _IssuesUITest
	{
		public override string Issue => "CheckBox UI Test";

		public CheckBoxUITest(TestDevice device)
		: base(device)
		{ }

		[Test]
		[Category(UITestCategories.CheckBox), Order(1)]
		public void VerifyCheckBoxCheckedState()
		{
			App.WaitForElement("CheckBox");
			App.Tap("CheckBox");
			VerifyScreenshot();
		}

		[Test]
		[Category(UITestCategories.CheckBox), Order(2)]
		public void VerifyCheckBoxUnCheckedState()
		{
			App.WaitForElement("CheckBox");
			App.Tap("CheckBox");
			VerifyScreenshot();
		}
	}
}

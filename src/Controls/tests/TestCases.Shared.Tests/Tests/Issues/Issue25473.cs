﻿#if !MACCATALYST
using Microsoft.Maui.Platform;
using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues
{
	public class Issue25473 : _IssuesUITest
	{
		public Issue25473(TestDevice testDevice) : base(testDevice)
		{
		}

		public override string Issue => "MAUI Entry in Windows always shows ClearButton despite ClearButtonVisibility set to 'Never'";

		[Test]
		[Category(UITestCategories.Entry)]
		public void VerifyEntryClearButtonVisibilitySetToNever()
		{
			App.WaitForElement("SecondaryEntryField");
			App.Tap("PrimaryEntryField");
			App.EnterText("PrimaryEntryField", "ClearButton is set to WhileEditing");
			App.Tap("SecondaryEntryField");
			App.EnterText("SecondaryEntryField", "ClearButton is set to Never");
			VerifyScreenshot();
		}
	}
}
#endif
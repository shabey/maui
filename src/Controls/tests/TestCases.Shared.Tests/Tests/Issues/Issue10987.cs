﻿using Microsoft.Maui.Platform;
using NUnit.Framework;
using UITest.Appium;
using UITest.Core;
namespace Microsoft.Maui.TestCases.Tests.Issues
{
	public class Issue10987 : _IssuesUITest
	{
		public Issue10987(TestDevice device) : base(device) { }

		public override string Issue => "Editor HorizontalTextAlignment Does not Works.";

		[Test]
		[Category(UITestCategories.Editor)]
		[FailsOnMac]
  		public void EditorPlaceholderRuntimeTextAlignmentChanged()
		{
			App.WaitForElement("Editor");
			App.Tap("Button");
			VerifyScreenshot();
		}

		[Test]
		[Category(UITestCategories.Editor)]
		[FailsOnMac]
		public void EditorRuntimeTextAlignmentChanged()
		{
			App.WaitForElement("Editor");
			App.EnterText("Editor", "Editor Text");
			App.Tap("ResetButton");
			VerifyScreenshot();
		}		
	}
}

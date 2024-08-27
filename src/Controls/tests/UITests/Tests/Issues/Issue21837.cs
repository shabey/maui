﻿using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.AppiumTests.Issues
{
	public class Issue21837 : _IssuesUITest
	{
		public Issue21837(TestDevice device) : base(device)
		{
		}

		public override string Issue => "Span's TapGestureRecognizer not working if text is truncated";

		[Test]
		public void TapsShouldBeCorrectlyRecognized()
		{
			int numberOfTappableLabels = 7;

			_ = App.WaitForElement("label1");

			for (int i = 0; i < numberOfTappableLabels; i++)
				App.Click($"label{i + 1}");

			var resultText = App.FindElement("resultLabel").GetText();
			Assert.AreEqual(resultText, $"Number of recognized taps: {numberOfTappableLabels}");
		}
	}
}

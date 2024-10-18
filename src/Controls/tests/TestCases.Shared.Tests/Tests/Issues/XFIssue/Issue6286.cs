﻿using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue6286 : _IssuesUITest
{
	public Issue6286(TestDevice testDevice) : base(testDevice)
	{
	}

	public override string Issue => "ObjectDisposedException in Android WebView.EvaluateJavascriptAsync ";

	//[Test]
	//[Category(UITestCategories.WebView)]
	//public void Issue6286_WebView_Test()
	//{
	//	RunningApp.QueryUntilPresent(() => RunningApp.WaitForElement("success"));
	//}
}
﻿
using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue12126 : _IssuesUITest
{
	public Issue12126(TestDevice testDevice) : base(testDevice)
	{
	}

	public override string Issue => "[iOS] TabBarIsVisible = True/False breaking for multiple nested pages";

	// Where does TapBackArrow() come from?
	// [Test]
	// [Category(UITestCategories.Shell)]
	// public void NavigatingBackFromMultiplePushPagesChangesTabVisibilityCorrectly()
	// {
	// 	App.WaitForElement("TestReady");
	// 	TapBackArrow();
	// 	App.WaitForElement("Tab 1");
	// }
}

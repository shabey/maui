﻿using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.AppiumTests.Issues
{
	public class RefreshViewTests : _IssuesUITest
	{
		public RefreshViewTests(TestDevice device)
			: base(device)
		{ }

		public override string Issue => "Refresh View Tests";

		[Test]
		public void IsRefreshingAndCommandTest()
		{
#if NATIVE_AOT
			Assert.Ignore("Crashes with System.NullReferenceException when running with NativeAOT, see https://github.com/dotnet/maui/issues/20553");
#endif
			App.Click("ToggleRefresh");
			Assert.IsTrue(App.WaitForTextToBePresentInElement("IsRefreshingLabel", "IsRefreshing: True"));
			Assert.IsTrue(App.WaitForTextToBePresentInElement("IsRefreshingLabel", "IsRefreshing: False"));
		}
	}
}

﻿#if IOS
using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues
{
	public class CarouselViewRemoveAt : _IssuesUITest
	{
		public CarouselViewRemoveAt(TestDevice device)
			: base(device)
		{
		}

		public override string Issue => "ObservableCollection.RemoveAt(index) with a valid index raises ArgumentOutOfRangeException";

		[Test]
		[Category(UITestCategories.CarouselView)]
#if TEST_FAILS_ON_IOS|| TEST_FAILS_ON_CATALYST
	[Ignore("Currently fails on iOS and mac; see https://github.com/dotnet/maui/issues/19488")]
#endif
		public void Issue10300Test()
		{
			App.Click("Add");
			App.Click("Delete");
			App.WaitForElement("Close");
			App.Click("Close");
			App.WaitForElement("2");
		}
	}
}
#endif
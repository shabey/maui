#if ANDROID // Facing a full exception when clicking on the already selected tab(App.Tap()) in Windows, iOS, and macOS, so excluded those platforms.
using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues
{
	public class Issue25599 : _IssuesUITest
	{
		public Issue25599(TestDevice testDevice) : base(testDevice){ }

		public override string Issue => "OnNavigating wrong target when tapping the same tab";

		[Test]
		[Category(UITestCategories.Navigation)]
		public void NavigatingEventFired()
		{
			App.WaitForElement("HomePageButton");
			App.Tap("HomePageButton");
			App.WaitForElement("DetailsPageLabel");
			App.Tap("Home"); // Tapping already selected Home tab
			App.WaitForElement("DetailsPageLabel");
			App.WaitForNoElement("HomePageLabel"); // Navigation does not occur when clicking on an already selected tab
		}
	}
}
#endif
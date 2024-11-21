
using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Bugzilla32040 : _IssuesUITest
{
	public Bugzilla32040(TestDevice testDevice) : base(testDevice)
	{
	}

	public override string Issue => "EntryCell.Tapped or SwitchCell.Tapped does not fire when within a TableView ";

	[Test]
	[Category(UITestCategories.Cells)]
	public void TappedWorksForEntryAndSwithCellTest()
	{ 
		App.WaitForElement("blahblah");
		App.Tap("blahblah");
		App.WaitForElement("Tapped");

		App.Tap("Click Here");
		Assert.That(App.WaitForElement("yaddayadda").GetText(), Is.EqualTo("Tapped"));
	}
}

using NUnit.Framework;
using Microsoft.Maui.Controls.CustomAttributes;

namespace Microsoft.Maui.Controls.Compatibility.UITests
{
	[TestFixture]
	[Category(UITestCategories.ActivityIndicator)]
	internal class ActivityIndicatorUITests : _ViewUITests
	{
		public ActivityIndicatorUITests()
		{
			PlatformViewType = Views.ActivityIndicator;
		}

		protected override void NavigateToGallery()
		{
			App.NavigateToGallery(GalleryQueries.ActivityIndicatorGallery);
		}

		// View tests
		[UiTestExempt(ExemptReason.CannotTest, "Invalid interaction")]
		public override void _Focus()
		{
		}

		public override void _GestureRecognizers()
		{
			// TODO Can implement this
			var remote = new ViewContainerRemote(App, Test.View.GestureRecognizers, PlatformViewType);
			remote.GoTo();
		}

		[UiTestExempt(ExemptReason.CannotTest, "Invalid interaction")]
		public override void _IsEnabled()
		{
		}

		[UiTestExempt(ExemptReason.CannotTest, "Invalid interaction")]
		public override void _IsFocused()
		{
		}

		[UiTestExempt(ExemptReason.CannotTest, "Invalid interaction")]
		public override void _UnFocus()
		{
		}

		// ActivityIndicator tests
		[Test]
		[UiTest(typeof(ActivityIndicator), "IsRunning")]
		[Category(UITestCategories.UwpIgnore)]
		public void IsRunning()
		{
			var remote = new ViewContainerRemote(App, Test.ActivityIndicator.IsRunning, PlatformViewType);
			remote.GoTo();
#if __MACOS__
			Assert.Inconclusive("Not tested yet");
#elif WINDOWS
			Assert.Inconclusive(PleaseInspect);
#else
			var isRunning = remote.GetProperty<bool>(ActivityIndicator.IsRunningProperty);
			Assert.IsTrue(isRunning);
#endif
		}

		protected override void FixtureTeardown()
		{
			App.NavigateBack();
			base.FixtureTeardown();
		}
	}
}
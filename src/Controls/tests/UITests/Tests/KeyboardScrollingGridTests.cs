﻿using Maui.Controls.Sample;
using NUnit.Framework;
using UITest.Core;

namespace Microsoft.Maui.AppiumTests
{
#if NATIVE_AOT
	[Ignore("Times out when running with NativeAOT, see https://github.com/dotnet/maui/issues/20553)")]
#endif
	public class KeyboardScrollingGridTests : UITest
	{
		const string KeyboardScrollingGallery = "Keyboard Scrolling Gallery - Grid with Star Row";
		public KeyboardScrollingGridTests(TestDevice device)
			: base(device)
		{
		}

		protected override void FixtureSetup()
		{
			base.FixtureSetup();
			App.NavigateToGallery(KeyboardScrollingGallery);
		}

		protected override void FixtureTeardown()
		{
			base.FixtureTeardown();
			this.Back();
		}

		[Test]
		public void GridStarRowScrollingTest()
		{
			this.IgnoreIfPlatforms(new TestDevice[] { TestDevice.Android, TestDevice.Mac, TestDevice.Windows }, KeyboardScrolling.IgnoreMessage);
			KeyboardScrolling.GridStarRowScrollingTest(App);
		}
	}
}

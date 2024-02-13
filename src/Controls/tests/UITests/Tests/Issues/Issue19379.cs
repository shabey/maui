﻿using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.AppiumTests.Issues
{
	public class Issue19379 : _IssuesUITest
	{
		public Issue19379(TestDevice device)
			: base(device)
		{ }

		public override string Issue => "Not able to update CollectionView header";

		[Test]
		public void UpdateCollectionViewHeaderTest()
		{
#if NATIVE_AOT
			Assert.Ignore("Fails with Snapshot different than baseline when running with NativeAOT, see https://github.com/dotnet/maui/issues/20553");
#endif
			this.IgnoreIfPlatforms(new TestDevice[] { TestDevice.Android, TestDevice.Mac, TestDevice.Windows });

			App.WaitForElement("WaitForStubControl");

			// 1. Update the CollectionView Header.
			App.Click("UpdateButton");

			// 2. Verify the result.
			VerifyScreenshot();
		}
	}
}

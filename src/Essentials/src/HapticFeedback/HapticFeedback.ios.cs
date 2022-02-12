using System;
using System.Threading.Tasks;
using ObjCRuntime;
using UIKit;

namespace Microsoft.Maui.Essentials.Implementations
{
	public partial class HapticFeedbackImplementation : IHapticFeedback
	{
		public bool IsSupported => true;

		public void Perform(HapticFeedbackType type)
		{
			switch (type)
			{
				case HapticFeedbackType.LongPress:
					LongPress();
					break;
				default:
					Click();
					break;
			}
		}

		static void Click()
		{
			if (Platform.HasOSVersion(10, 0))
			{
				var impact = new UIImpactFeedbackGenerator(UIImpactFeedbackStyle.Light);
				impact.Prepare();
				impact.ImpactOccurred();
				impact.Dispose();
			}
		}

		public void LongPress()
		{
			if (Platform.HasOSVersion(10, 0))
			{
				var impact = new UIImpactFeedbackGenerator(UIImpactFeedbackStyle.Medium);
				impact.Prepare();
				impact.ImpactOccurred();
				impact.Dispose();
			}
		}
	}
}

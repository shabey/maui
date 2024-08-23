using UIKit;

namespace Microsoft.Maui.Platform
{
	public class LayoutView : MauiView
	{
		public override void SubviewAdded(UIView uiview)
		{
			InvalidateConstraintsCache();
			base.SubviewAdded(uiview);
			this.GetSuperViewIfWindowSet()?.SetNeedsLayout();
		}

		public override void WillRemoveSubview(UIView uiview)
		{
			InvalidateConstraintsCache();
			base.WillRemoveSubview(uiview);
			this.GetSuperViewIfWindowSet()?.SetNeedsLayout();
		}
	}
}
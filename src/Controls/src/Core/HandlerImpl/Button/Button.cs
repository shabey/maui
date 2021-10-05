﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Handlers;

namespace Microsoft.Maui.Controls
{
	public partial class Button
	{
		public new static void RemapForControls()
		{
			// IButton does not include the ContentType property, so we map it here to handle Image Positioning

			IPropertyMapper<IButton, ButtonHandler> ControlsButtonMapper = new PropertyMapper<Button, ButtonHandler>(ButtonHandler.Mapper)
			{
				[nameof(ContentLayout)] = MapContentLayout,
#if __IOS__
				[nameof(Padding)] = MapPadding,
#endif
#if WINDOWS
			[nameof(IText.Text)] = MapText,
			[nameof(IButton.ImageSource)] = MapImageSource
#endif
			};

			ButtonHandler.Mapper = ControlsButtonMapper;
		}

		public static void MapContentLayout(ButtonHandler handler, Button button)
		{
			handler.NativeView.UpdateContentLayout(button);
		}

//#if WINDOWS
//		protected override Graphics.Size ArrangeOverride(Graphics.Rectangle bounds)
//		{
//			var result = base.ArrangeOverride(bounds);
//			Handler?.UpdateValue(nameof(ContentLayout));
//			return result;
//		}
//#endif
	}
}

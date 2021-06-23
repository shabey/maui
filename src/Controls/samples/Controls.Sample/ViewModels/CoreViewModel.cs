﻿using System.Collections.Generic;
using Maui.Controls.Sample.Models;
using Maui.Controls.Sample.Pages;
using Maui.Controls.Sample.ViewModels.Base;

namespace Maui.Controls.Sample.ViewModels
{
	public class CoreViewModel : BaseGalleryViewModel
	{
		protected override IEnumerable<SectionModel> CreateItems() => new[]
		{
			new SectionModel(typeof(BrushesPage), "Brushes",
				"A brush enables you to paint an area, such as the background of a control, using different approaches."),

			new SectionModel(typeof(SemanticsPage), "Semantics",
				".NET MAUI allows accessibility values to be set on user interface elements by using Semantics values."),

			new SectionModel(typeof(TransformationsPage), "Transformations",
				"Apply scale transformations, rotation, etc. to a View."),
		};
	}
}
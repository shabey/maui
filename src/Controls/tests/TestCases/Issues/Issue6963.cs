﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Controls.CustomAttributes;
using Microsoft.Maui.Controls.Internals;

namespace Maui.Controls.Sample.Issues
{
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 6963, "[Bug] CollectionView multiple pre-selection throws ArgumentOutOfRangeException when SelectedItems is bound to an ObservableCollection initialized inside the constructor.",
		PlatformAffected.iOS | PlatformAffected.UWP)]
	public class Issue6963 : TestNavigationPage
	{
		protected override void Init()
		{
			PushAsync(new CollectionViewGalleries.SelectionGalleries.SelectionSynchronization());
		}
	}
}
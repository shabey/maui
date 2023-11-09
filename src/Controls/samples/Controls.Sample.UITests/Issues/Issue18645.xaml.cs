﻿using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace Maui.Controls.Sample.Issues
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	[Issue(IssueTracker.ManualTest, "E5", "Editor MaxLines property works as expected", PlatformAffected.All)]
	public partial class Issue18645 : ContentPage
	{
		public Issue18645()
		{
			InitializeComponent();
		}
	}
}
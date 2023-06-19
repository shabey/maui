﻿using Microsoft.Maui.Controls.Xaml;

namespace Microsoft.Maui.Controls.ControlGallery.GalleryPages.SwipeViewGalleries
{
	[XamlCompilation(XamlCompilationOptions.Skip)]
	public partial class VerticalSwipeThresholdGallery : ContentPage
	{
		public VerticalSwipeThresholdGallery()
		{
			InitializeComponent();
		}

		void OnThresholdRevealSliderChanged(object sender, ValueChangedEventArgs args)
		{
			RevealThresholdSwipeView.Close();
		}

		void OnThresholdExecuteSliderChanged(object sender, ValueChangedEventArgs args)
		{
			ExecuteThresholdSwipeView.Close();
		}
	}
}
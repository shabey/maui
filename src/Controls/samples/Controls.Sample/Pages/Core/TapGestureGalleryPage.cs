﻿using Maui.Controls.Sample.Pages.Base;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Maui.Controls.Sample.Pages
{
	public class TapGestureGalleryPage : BasePage
	{
		Command TapCommand;
		Label changeColorBoxView;

		public TapGestureGalleryPage()
		{
			TapCommand = new Command<Color>(HandleTapCommand);
			var vertical = new VerticalStackLayout
			{
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center,
				Spacing = 40
			};

			var horizontal = new HorizontalStackLayout
			{
				Spacing = 20,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			};
			vertical.Add(horizontal);

			var singleTapLabel = new Label
			{
				Text = "Tap me!",
				BackgroundColor = Colors.PaleGreen
			};
			var singleTapGesture = new TapGestureRecognizer
			{
				Command = TapCommand,
				CommandParameter = Colors.PaleGreen,
				NumberOfTapsRequired = 1,
			};
			singleTapLabel.GestureRecognizers.Add(singleTapGesture);
			horizontal.Add(singleTapLabel);

			var doubleTapLabel = new Label
			{
				Text = "Double Tap me!!",
				BackgroundColor = Colors.Aqua
			};
			var doubleTapGesture = new TapGestureRecognizer
			{
				Command = TapCommand,
				CommandParameter = Colors.Aqua,
				NumberOfTapsRequired = 2,
			};
			doubleTapLabel.GestureRecognizers.Add(doubleTapGesture);
			horizontal.Add(doubleTapLabel);

			changeColorBoxView = new Label
			{
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				WidthRequest = 200,
				HeightRequest = 50,
				Text = "Tap Gesture Gallery"
			};
			vertical.Add(changeColorBoxView);
			Content = vertical;
		}

		void HandleTapCommand(Color backgroundColor)
		{
			changeColorBoxView.BackgroundColor = backgroundColor;
		}
	}
}

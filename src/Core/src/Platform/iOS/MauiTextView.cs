﻿using System;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace Microsoft.Maui.Platform
{
	public class MauiTextView : UITextView
	{
		readonly UILabel _placeholderLabel;

		public MauiTextView()
		{
			_placeholderLabel = InitPlaceholderLabel();
			Changed += OnChanged;
		}

		public MauiTextView(CGRect frame)
			: base(frame)
		{
			_placeholderLabel = InitPlaceholderLabel();
			Changed += OnChanged;
		}

		// Native Changed doesn't fire when the Text Property is set in code
		// We use this event as a way to fire changes whenever the Text changes
		// via code or user interaction.
		public event EventHandler? TextSetOrChanged;

		public string? PlaceholderText
		{
			get => _placeholderLabel.Text;
			set
			{
				_placeholderLabel.Text = value;
				_placeholderLabel.SizeToFit();
			}
		}

		public NSAttributedString? AttributedPlaceholderText
		{
			get => _placeholderLabel.AttributedText;
			set
			{
				_placeholderLabel.AttributedText = value;
				_placeholderLabel.SizeToFit();
			}
		}

		public UIColor? PlaceholderTextColor
		{
			get => _placeholderLabel.TextColor;
			set => _placeholderLabel.TextColor = value;
		}

		public override string? Text
		{
			get => base.Text;
			set
			{
				var old = base.Text;

				base.Text = value;

				if (old != value)
				{
					HidePlaceholderIfTextIsPresent(value);
					TextSetOrChanged?.Invoke(this, EventArgs.Empty);
				}
			}
		}

		public override NSAttributedString AttributedText
		{
			get => base.AttributedText;
			set
			{
				var old = base.AttributedText;

				base.AttributedText = value;

				if (old?.Value != value?.Value)
				{
					HidePlaceholderIfTextIsPresent(value?.Value);
					TextSetOrChanged?.Invoke(this, EventArgs.Empty);
				}
			}
		}

		UILabel InitPlaceholderLabel()
		{
			var placeholderLabel = new UILabel
			{
				BackgroundColor = UIColor.Clear,
				TextColor = ColorExtensions.PlaceholderColor,
				Lines = 0
			};

			AddSubview(placeholderLabel);

			var edgeInsets = TextContainerInset;
			var lineFragmentPadding = TextContainer.LineFragmentPadding;

			var vConstraints = NSLayoutConstraint.FromVisualFormat(
				"V:|-" + edgeInsets.Top + "-[PlaceholderLabel]-" + edgeInsets.Bottom + "-|", 0, new NSDictionary(),
				NSDictionary.FromObjectsAndKeys(
					new NSObject[] { placeholderLabel }, new NSObject[] { new NSString("PlaceholderLabel") })
			);

			var hConstraints = NSLayoutConstraint.FromVisualFormat(
				"H:|-" + lineFragmentPadding + "-[PlaceholderLabel]-" + lineFragmentPadding + "-|",
				0, new NSDictionary(),
				NSDictionary.FromObjectsAndKeys(
					new NSObject[] { placeholderLabel }, new NSObject[] { new NSString("PlaceholderLabel") })
			);

			placeholderLabel.TranslatesAutoresizingMaskIntoConstraints = false;

			AddConstraints(hConstraints);
			AddConstraints(vConstraints);

			return placeholderLabel;
		}

		void HidePlaceholderIfTextIsPresent(string? value)
		{
			_placeholderLabel.Hidden = !string.IsNullOrEmpty(value);
		}

		void OnChanged(object? sender, EventArgs e)
		{
			HidePlaceholderIfTextIsPresent(Text);
			TextSetOrChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}
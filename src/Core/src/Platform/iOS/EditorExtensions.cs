using UIKit;
using Microsoft.Maui.Platform.iOS;

namespace Microsoft.Maui
{
	public static class EditorExtensions
	{
		public static void UpdateText(this UITextView textView, IEditor editor)
		{
			string text = editor.Text;

			if (textView.Text != text)
			{
				textView.Text = text;
			}
		}

		public static void UpdatePlaceholder(this MauiTextView textView, IEditor editor)
		{
			textView.PlaceholderText = editor.Placeholder;
		}

		public static void UpdatePlaceholderColor(this MauiTextView textView, IEditor editor, UIColor? defaultPlaceholderColor)
		{
			Color placeholderColor = editor.PlaceholderColor;
			if (placeholderColor.IsDefault)
				textView.PlaceholderTextColor = defaultPlaceholderColor;
			else
				textView.PlaceholderTextColor = placeholderColor.ToNative();
		}
	}
}

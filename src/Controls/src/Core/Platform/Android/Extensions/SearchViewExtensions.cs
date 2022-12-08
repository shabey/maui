﻿using Android.Widget;
using Microsoft.Maui.Controls.Internals;
using SearchView = AndroidX.AppCompat.Widget.SearchView;

namespace Microsoft.Maui.Controls.Platform
{
	public static class SearchViewExtensions
	{
		public static void UpdateText(this SearchView searchView, InputView inputView)
		{
			if (inputView.TextTransform == TextTransform.Default || inputView.TextTransform == TextTransform.None)
				return;

			searchView.SetQuery((TextTransformUtilites.GetTransformedText(inputView.Text, inputView.TextTransform)), false);
		}
	}
}

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Maui.Controls.Xaml.Internals;
using AView = Android.Views.View;

[assembly: Microsoft.Maui.Controls.Dependency(typeof(Microsoft.Maui.Controls.Compatibility.Platform.Android.NativeBindingService))]

namespace Microsoft.Maui.Controls.Compatibility.Platform.Android
{
	class NativeBindingService : INativeBindingService
	{
		[UnconditionalSuppressMessage("Trimming", "IL2075", Justification = TrimmerConstants.NativeBindingService)]
		[UnconditionalSuppressMessage("Trimming", "IL2026", Justification = TrimmerConstants.NativeBindingService)]
		public bool TrySetBinding(object target, string propertyName, BindingBase binding)
		{
			var view = target as AView;
			if (view == null)
				return false;
			if (target.GetType().GetProperty(propertyName)?.GetMethod == null)
				return false;
			view.SetBinding(propertyName, binding);
			return true;
		}

		public bool TrySetBinding(object target, BindableProperty property, BindingBase binding)
		{
			var view = target as AView;
			if (view == null)
				return false;
			view.SetBinding(property, binding);
			return true;
		}

		public bool TrySetValue(object target, BindableProperty property, object value)
		{
			var view = target as AView;
			if (view == null)
				return false;
			view.SetValue(property, value);
			return true;
		}
	}
}
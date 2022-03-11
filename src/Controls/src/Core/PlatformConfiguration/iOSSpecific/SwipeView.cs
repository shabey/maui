namespace Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific
{
	using FormsElement = Maui.Controls.SwipeView;

	/// <include file="../../../../docs/Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific/SwipeView.xml" path="Type[@FullName='Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific.SwipeView']/Docs" />
	public static class SwipeView
	{
		/// <include file="../../../../docs/Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific/SwipeView.xml" path="//Member[@MemberName='SwipeTransitionModeProperty']/Docs" />
		public static readonly BindableProperty SwipeTransitionModeProperty = BindableProperty.Create("SwipeTransitionMode", typeof(SwipeTransitionMode), typeof(SwipeView), SwipeTransitionMode.Reveal);

		/// <include file="../../../../docs/Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific/SwipeView.xml" path="//Member[@MemberName='GetSwipeTransitionMode' and position()=0]/Docs" />
		public static SwipeTransitionMode GetSwipeTransitionMode(BindableObject element)
		{
			return (SwipeTransitionMode)element.GetValue(SwipeTransitionModeProperty);
		}

		/// <include file="../../../../docs/Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific/SwipeView.xml" path="//Member[@MemberName='SetSwipeTransitionMode' and position()=0]/Docs" />
		public static void SetSwipeTransitionMode(BindableObject element, SwipeTransitionMode value)
		{
			element.SetValue(SwipeTransitionModeProperty, value);
		}

		/// <include file="../../../../docs/Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific/SwipeView.xml" path="//Member[@MemberName='GetSwipeTransitionMode']/Docs" />
		public static SwipeTransitionMode GetSwipeTransitionMode(this IPlatformElementConfiguration<iOS, FormsElement> config)
		{
			return GetSwipeTransitionMode(config.Element);
		}

		/// <include file="../../../../docs/Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific/SwipeView.xml" path="//Member[@MemberName='SetSwipeTransitionMode']/Docs" />
		public static IPlatformElementConfiguration<iOS, FormsElement> SetSwipeTransitionMode(this IPlatformElementConfiguration<iOS, FormsElement> config, SwipeTransitionMode value)
		{
			SetSwipeTransitionMode(config.Element, value);
			return config;
		}
	}
}
using Context = global::Android.Content.Context;

namespace Microsoft.Maui.DeviceTests
{
	public partial class Platform
	{
		public static Context DefaultContext { get; private set; }

		public static void Init(Context context)
		{
			DefaultContext = context;
		}
	}
}
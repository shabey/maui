using System;

namespace Microsoft.Maui.Devices.Sensors
{
	public partial class BarometerImplementation : IBarometer
	{
		void PlatformStart(SensorSpeed sensorSpeed)
			=> throw ExceptionUtils.NotSupportedOrImplementedException;

		void PlatformStop()
			=> throw ExceptionUtils.NotSupportedOrImplementedException;

		bool PlatformIsSupported
			=> throw ExceptionUtils.NotSupportedOrImplementedException;
	}
}

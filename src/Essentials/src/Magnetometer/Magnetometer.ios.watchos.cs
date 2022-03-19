using CoreMotion;
using Foundation;

namespace Microsoft.Maui.Devices.Sensors
{
	partial class MagnetometerImplementation : IMagnetometer
	{
		bool PlatformIsSupported =>
			Platform.MotionManager?.MagnetometerAvailable ?? false;

		void PlatformStart(SensorSpeed sensorSpeed)
		{
			var manager = Platform.MotionManager;
			manager.MagnetometerUpdateInterval = sensorSpeed.ToPlatform();
			manager.StartMagnetometerUpdates(Platform.GetCurrentQueue(), DataUpdated);
		}

		void DataUpdated(CMMagnetometerData data, NSError error)
		{
			if (data == null)
				return;

			var field = data.MagneticField;
			var magnetometerData = new MagnetometerData(field.X, field.Y, field.Z);
			RaiseReadingChanged(magnetometerData);
		}

		void PlatformStop() =>
			Platform.MotionManager?.StopMagnetometerUpdates();
	}
}

using Tizen.Sensor;
using TizenAccelerometer = Tizen.Sensor.Accelerometer;

namespace Microsoft.Maui.Essentials
{
	public partial class AccelerometerImpl
	{
		internal static TizenAccelerometer DefaultSensor =>
			(TizenAccelerometer)Platform.GetDefaultSensor(SensorType.Accelerometer);

		internal static bool IsSupported =>
			TizenAccelerometer.IsSupported;

		private void PlatformStart(SensorSpeed sensorSpeed)
		{
			DefaultSensor.Interval = sensorSpeed.ToPlatform();
			DefaultSensor.DataUpdated += DataUpdated;
			DefaultSensor.Start();
		}

		private void PlatformStop()
		{
			DefaultSensor.DataUpdated -= DataUpdated;
			DefaultSensor.Stop();
		}

		private void DataUpdated(object sender, AccelerometerDataUpdatedEventArgs e)
		{
			OnChanged(new AccelerometerData(e.X, e.Y, e.Z));
		}
	}
}

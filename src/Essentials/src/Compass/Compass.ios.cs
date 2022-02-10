using CoreLocation;

namespace Microsoft.Maui.Essentials.Implementations
{
	public partial class CompassImplementation : ICompass
	{
		// The angular distance is measured relative to the last delivered heading event. Align with UWP numbers
		internal const double FastestFilter = .01;
		internal const double GameFilter = .5;
		internal const double NormalFilter = 1;
		internal const double UIFilter = 2;

		public bool ShouldDisplayHeadingCalibration { get; set; } = false;

		public bool IsSupported =>
			CLLocationManager.HeadingAvailable;

		public bool IsMonitoring { get; set; }

		CLLocationManager locationManager;

		public void Start(SensorSpeed sensorSpeed)
			=> Start(sensorSpeed, false);

		public void Start(SensorSpeed sensorSpeed, bool applyLowPassFilter)
		{
			locationManager = new CLLocationManager();
			locationManager.ShouldDisplayHeadingCalibration += LocationManagerShouldDisplayHeadingCalibration;
			switch (sensorSpeed)
			{
				case SensorSpeed.Fastest:
					locationManager.HeadingFilter = FastestFilter;
					locationManager.DesiredAccuracy = CLLocation.AccurracyBestForNavigation;
					break;
				case SensorSpeed.Game:
					locationManager.HeadingFilter = GameFilter;
					locationManager.DesiredAccuracy = CLLocation.AccurracyBestForNavigation;
					break;
				case SensorSpeed.Default:
					locationManager.HeadingFilter = NormalFilter;
					locationManager.DesiredAccuracy = CLLocation.AccuracyBest;
					break;
				case SensorSpeed.UI:
					locationManager.HeadingFilter = UIFilter;
					locationManager.DesiredAccuracy = CLLocation.AccuracyBest;
					break;
			}

			locationManager.UpdatedHeading += LocationManagerUpdatedHeading;
			locationManager.StartUpdatingHeading();
		}

		bool LocationManagerShouldDisplayHeadingCalibration(CLLocationManager manager) => ShouldDisplayHeadingCalibration;

		void LocationManagerUpdatedHeading(object sender, CLHeadingUpdatedEventArgs e)
		{
			var data = new CompassData(e.NewHeading.MagneticHeading);
			Compass.OnChanged(data);
		}

		public void Stop()
		{
			if (locationManager == null)
				return;

			locationManager.ShouldDisplayHeadingCalibration -= LocationManagerShouldDisplayHeadingCalibration;
			locationManager.UpdatedHeading -= LocationManagerUpdatedHeading;
			locationManager.StopUpdatingHeading();
			locationManager.Dispose();
			locationManager = null;
		}
	}
}

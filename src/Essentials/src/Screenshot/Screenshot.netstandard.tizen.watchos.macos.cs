using System.IO;
using System.Threading.Tasks;

namespace Microsoft.Maui.Media
{
	public partial class ScreenshotImplementation : IScreenshot
	{
		public bool IsCaptureSupported =>
			throw ExceptionUtils.NotSupportedOrImplementedException;

		public Task<IScreenshotResult> CaptureAsync() =>
			throw ExceptionUtils.NotSupportedOrImplementedException;
	}

	partial class ScreenshotResult
	{
		ScreenshotResult()
		{
		}

		Task<Stream> PlatformOpenReadAsync(ScreenshotFormat format, int quality) =>
			throw ExceptionUtils.NotSupportedOrImplementedException;

		Task PlatformCopyToAsync(Stream destination, ScreenshotFormat format, int quality) =>
			throw ExceptionUtils.NotSupportedOrImplementedException;

		Task<byte[]> PlatformToPixelBufferAsync() =>
			throw ExceptionUtils.NotSupportedOrImplementedException;
	}
}

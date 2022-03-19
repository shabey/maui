using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;

namespace Microsoft.Maui.Media
{
	public interface IMediaPicker
	{
		bool IsCaptureSupported { get; }

		Task<FileResult> PickPhotoAsync(MediaPickerOptions options);

		Task<FileResult> CapturePhotoAsync(MediaPickerOptions options);

		Task<FileResult> PickVideoAsync(MediaPickerOptions options);

		Task<FileResult> CaptureVideoAsync(MediaPickerOptions options);
	}
}
namespace Microsoft.Maui.Essentials
{
	/// <include file="../../docs/Microsoft.Maui.Essentials/MediaPicker.xml" path="Type[@FullName='Microsoft.Maui.Essentials.MediaPicker']/Docs" />
	public static partial class MediaPicker
	{
		/// <include file="../../docs/Microsoft.Maui.Essentials/MediaPicker.xml" path="//Member[@MemberName='IsCaptureSupported']/Docs" />
		public static bool IsCaptureSupported
			=> Current.IsCaptureSupported;

		/// <include file="../../docs/Microsoft.Maui.Essentials/MediaPicker.xml" path="//Member[@MemberName='PickPhotoAsync']/Docs" />
		public static Task<FileResult> PickPhotoAsync(MediaPickerOptions options = null) =>
			Current.PickPhotoAsync(options);

		/// <include file="../../docs/Microsoft.Maui.Essentials/MediaPicker.xml" path="//Member[@MemberName='CapturePhotoAsync']/Docs" />
		public static Task<FileResult> CapturePhotoAsync(MediaPickerOptions options = null)
		{
			if (!IsCaptureSupported)
				throw new FeatureNotSupportedException();

			return Current.CapturePhotoAsync(options);
		}

		/// <include file="../../docs/Microsoft.Maui.Essentials/MediaPicker.xml" path="//Member[@MemberName='PickVideoAsync']/Docs" />
		public static Task<FileResult> PickVideoAsync(MediaPickerOptions options = null) =>
			Current.PickVideoAsync(options);

		/// <include file="../../docs/Microsoft.Maui.Essentials/MediaPicker.xml" path="//Member[@MemberName='CaptureVideoAsync']/Docs" />
		public static Task<FileResult> CaptureVideoAsync(MediaPickerOptions options = null)
		{
			if (!IsCaptureSupported)
				throw new FeatureNotSupportedException();

			return Current.CaptureVideoAsync(options);
		}

#nullable enable
		static IMediaPicker? currentImplementation;

		public static IMediaPicker Current =>
			currentImplementation ??= new MediaPickerImplementation();

		internal static void SetCurrent(IMediaPicker? implementation) =>
			currentImplementation = implementation;
#nullable disable
	}
}
namespace Microsoft.Maui.Media
{
	/// <include file="../../docs/Microsoft.Maui.Essentials/MediaPickerOptions.xml" path="Type[@FullName='Microsoft.Maui.Essentials.MediaPickerOptions']/Docs" />
	public class MediaPickerOptions
	{
		/// <include file="../../docs/Microsoft.Maui.Essentials/MediaPickerOptions.xml" path="//Member[@MemberName='Title']/Docs" />
		public string Title { get; set; }
	}
}

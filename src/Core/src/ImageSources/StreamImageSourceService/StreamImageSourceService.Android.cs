﻿#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Runtime;
using Bumptech.Glide;
using Bumptech.Glide.Load.Engine;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.BumptechGlide;

namespace Microsoft.Maui
{
	public partial class StreamImageSourceService
	{
		public override async Task<IImageSourceServiceResult<Drawable>?> LoadDrawableAsync(IImageSource imageSource, Android.Widget.ImageView imageView, CancellationToken cancellationToken = default)
		{
			if (imageSource is IStreamImageSource streamImageSource)
			{
				try
				{
					var stream = await streamImageSource.GetStreamAsync(cancellationToken).ConfigureAwait(false);

					// We can use the .NET stream directly because we register the InputStreamModelLoader.
					// There are 2 alternatives:
					//  - Load the bitmap manually and pass that along, but then we do not get the decoding features.
					//  - Copy the stream into a byte array and that is double memory usage - especially for large streams.
					var inputStream = new InputStreamAdapter(stream);

					var listener = new RequestBuilderExtensions.RequestCompleteListener();
					var glide = Glide.With(imageView.Context);
					var builder = glide
						.Load(inputStream)
						.AddListener(listener);

					// Load into the image view
					var viewTarget = builder
						.Into(imageView);

					// Wait for the result from the listener
					var result = await listener.Result.ConfigureAwait(false);

					return new ImageSourceServiceResult(result, () => glide.Clear(viewTarget));
				}
				catch (Exception ex)
				{
					Logger?.LogWarning(ex, "Unable to load image stream.");
					throw;
				}
			}
			return null;
		}

		public override Task<IImageSourceServiceResult<Drawable>?> GetDrawableAsync(IImageSource imageSource, Context context, CancellationToken cancellationToken = default) =>
			GetDrawableAsync((IStreamImageSource)imageSource, context, cancellationToken);

		public async Task<IImageSourceServiceResult<Drawable>?> GetDrawableAsync(IStreamImageSource imageSource, Context context, CancellationToken cancellationToken = default)
		{
			if (imageSource.IsEmpty)
				return null;

			try
			{
				var stream = await imageSource.GetStreamAsync(cancellationToken).ConfigureAwait(false);

				// We can use the .NET stream directly because we register the InputStreamModelLoader.
				// There are 2 alternatives:
				//  - Load the bitmap manually and pass that along, but then we do not get the decoding features.
				//  - Copy the stream into a byte array and that is double memory usage - especially for large streams.
				var inputStream = new InputStreamAdapter(stream);

				var result = await Glide
					.With(context)
					.Load(inputStream)
					.SetDiskCacheStrategy(DiskCacheStrategy.None)
					.SubmitAsync(context, cancellationToken)
					.ConfigureAwait(false);

				if (result == null)
					throw new InvalidOperationException("Unable to load image stream.");

				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogWarning(ex, "Unable to load image stream.");
				throw;
			}
		}
	}
}
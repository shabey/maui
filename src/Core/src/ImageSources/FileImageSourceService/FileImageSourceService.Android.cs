﻿#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics.Drawables;
using Bumptech.Glide;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.BumptechGlide;

namespace Microsoft.Maui
{
	public partial class FileImageSourceService
	{
		public override async Task<IImageSourceServiceResult<Drawable>?> LoadDrawableAsync(IImageSource imageSource, Android.Widget.ImageView imageView, CancellationToken cancellationToken = default)
		{
			if (imageSource is IFileImageSource fileImageSource)
			{
				try
				{
					var listener = new RequestBuilderExtensions.RequestCompleteListener();
					var glide = Glide.With(imageView.Context);
					var builder = glide
						.Load(fileImageSource.File, imageView.Context!)
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
			GetDrawableAsync((IFileImageSource)imageSource, context, cancellationToken);

		public async Task<IImageSourceServiceResult<Drawable>?> GetDrawableAsync(IFileImageSource imageSource, Context context, CancellationToken cancellationToken = default)
		{
			if (imageSource.IsEmpty)
				return null;

			var filename = imageSource.File;

			try
			{
				ImageSourceServiceResult? result = null;
				var id = context.GetDrawableId(filename);
				if (id > 0)
				{
					var drawable = context.GetDrawable(id);
					if (drawable != null)
					{
						result = new ImageSourceServiceResult(drawable);
					}
				}

				if (result == null)
				{
					result = await Glide
						.With(context)
						.Load(filename, context)
						.SubmitAsync(context, cancellationToken)
						.ConfigureAwait(false);
				}

				if (result == null)
					throw new InvalidOperationException("Unable to load image file.");

				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogWarning(ex, "Unable to load image file '{File}'.", filename);
				throw;
			}
		}
	}
}
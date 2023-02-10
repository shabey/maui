﻿using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System;

namespace Microsoft.Maui.Resizetizer
{
	internal class AndroidAdaptiveIconGenerator
	{
		public AndroidAdaptiveIconGenerator(ResizeImageInfo info, string appIconName, string intermediateOutputPath, ILogger logger)
		{
			Info = info;
			Logger = logger;
			IntermediateOutputPath = intermediateOutputPath;
			AppIconName = appIconName;
		}

		public ResizeImageInfo Info { get; }

		public string IntermediateOutputPath { get; }

		public ILogger Logger { get; private set; }

		public string AppIconName { get; }

		const string AdaptiveIconDrawableXml =
@"<?xml version=""1.0"" encoding=""utf-8""?>
<adaptive-icon xmlns:android=""http://schemas.android.com/apk/res/android"">
	<background android:drawable=""@mipmap/{name}_background""/>
	<foreground android:drawable=""@mipmap/{name}_foreground""/>
	<monochrome android:drawable=""@mipmap/{name}_foreground"" />
</adaptive-icon>";

		public IEnumerable<ResizedImageInfo> Generate()
		{
			var sw = new Stopwatch();
			sw.Start();

			var results = new List<ResizedImageInfo>();

			var fullIntermediateOutputPath = new DirectoryInfo(IntermediateOutputPath);

			ProcessBackground(results, fullIntermediateOutputPath);
			ProcessForeground(results, fullIntermediateOutputPath);
			ProcessAdaptiveIcon(results, fullIntermediateOutputPath);

			sw.Stop();
			Logger?.Log($"Generating app icon took {sw.ElapsedMilliseconds}ms");

			return results;
		}

		void ProcessBackground(List<ResizedImageInfo> results, DirectoryInfo fullIntermediateOutputPath)
		{
			var backgroundFile = Info.Filename;
			(bool Exists, DateTime Modified) backgroundExists = Utils.FileExists(backgroundFile);
			var backgroundDestFilename = AppIconName + "_background.png";

			if (backgroundExists.Exists)
				Logger.Log("Converting Background SVG to PNG: " + backgroundFile);
			else
				Logger.Log("Background was not found (will manufacture): " + backgroundFile);

			foreach (var dpi in DpiPath.Android.AppIconParts)
			{
				var dir = Path.Combine(fullIntermediateOutputPath.FullName, dpi.Path);
				var destination = Path.Combine(dir, backgroundDestFilename);
				(bool Exists, DateTime Modified) destinationExists = Utils.FileExists(destination);
				Directory.CreateDirectory(dir);

				if (destinationExists.Modified > backgroundExists.Modified) {
					Logger.Log($"Skipping `{backgroundFile}` => `{destination}` file is up to date.");
					results.Add(new ResizedImageInfo { Dpi = dpi, Filename = destination });
					continue;
				}
	
				Logger.Log($"App Icon Background Part: " + destination);

				if (backgroundExists.Exists)
				{
					// resize the background
					var tools = SkiaSharpTools.Create(Info.IsVector, Info.Filename, dpi.Size, Info.Color, null, Logger);
					tools.Resize(dpi, destination, dpiSizeIsAbsolute: true);
				}
				else
				{
					// manufacture
					var tools = SkiaSharpTools.CreateImaginary(Info.Color, Logger);
					tools.Resize(dpi, destination);
				}

				results.Add(new ResizedImageInfo { Dpi = dpi, Filename = destination });
			}
		}

		void ProcessForeground(List<ResizedImageInfo> results, DirectoryInfo fullIntermediateOutputPath)
		{
			var foregroundFile = Info.ForegroundFilename;
			(bool Exists, DateTime Modified) foregroundExists = Utils.FileExists(foregroundFile);
			var foregroundDestFilename = AppIconName + "_foreground.png";

			if (foregroundExists.Exists)
				Logger.Log("Converting Foreground SVG to PNG: " + foregroundFile);
			else
				Logger.Log("Foreground was not found (will manufacture): " + foregroundFile);

			foreach (var dpi in DpiPath.Android.AppIconParts)
			{
				var dir = Path.Combine(fullIntermediateOutputPath.FullName, dpi.Path);
				var destination = Path.Combine(dir, foregroundDestFilename);
				(bool Exists, DateTime Modified) destinationExists = Utils.FileExists(destination);
				Directory.CreateDirectory(dir);

				if (destinationExists.Modified > foregroundExists.Modified) {
					Logger.Log($"Skipping `{foregroundFile}` => `{destination}` file is up to date.");
					results.Add(new ResizedImageInfo { Dpi = dpi, Filename = destination });
					continue;
				}

				Logger.Log($"App Icon Foreground Part: " + destination);

				if (foregroundExists.Exists)
				{
					// resize the forground
					var tools = SkiaSharpTools.Create(Info.ForegroundIsVector, Info.ForegroundFilename, dpi.Size, null, Info.TintColor, Logger);
					tools.Resize(dpi, destination, Info.ForegroundScale, dpiSizeIsAbsolute: true);
				}
				else
				{
					// manufacture
					var tools = SkiaSharpTools.CreateImaginary(null, Logger);
					tools.Resize(dpi, destination);
				}

				results.Add(new ResizedImageInfo { Dpi = dpi, Filename = destination });
			}
		}

		void ProcessAdaptiveIcon(List<ResizedImageInfo> results, DirectoryInfo fullIntermediateOutputPath)
		{
			var adaptiveIconXmlStr = AdaptiveIconDrawableXml
				.Replace("{name}", AppIconName);

			var dir = Path.Combine(fullIntermediateOutputPath.FullName, "mipmap-anydpi-v26");
			var adaptiveIconDestination = Path.Combine(dir, AppIconName + ".xml");
			var adaptiveIconRoundDestination = Path.Combine(dir, AppIconName + "_round.xml");
			Directory.CreateDirectory(dir);

			// Write out the adaptive icon xml drawables
			if (!File.Exists(adaptiveIconDestination))
				File.WriteAllText(adaptiveIconDestination, adaptiveIconXmlStr);
			if (!File.Exists(adaptiveIconRoundDestination))
				File.WriteAllText(adaptiveIconRoundDestination, adaptiveIconXmlStr);

			results.Add(new ResizedImageInfo { Dpi = new DpiPath("mipmap-anydpi-v26", 1), Filename = adaptiveIconDestination });
			results.Add(new ResizedImageInfo { Dpi = new DpiPath("mipmap-anydpi-v26", 1, "_round"), Filename = adaptiveIconRoundDestination });
		}
	}
}

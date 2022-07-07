﻿using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls.Build.Tasks;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Graphics;
using Mono.Cecil.Cil;

namespace Microsoft.Maui.Controls.XamlC;

class BrushTypeConverter : ICompiledTypeConverter
{
	public IEnumerable<Instruction> ConvertFromString(string value, ILContext context, BaseNode node)
	{
		var module = context.Body.Method.Module;

		do
		{
			if (string.IsNullOrEmpty(value))
				break;

			value = value.Trim();

			if (value.StartsWith("#", StringComparison.Ordinal))
			{
				var color = Color.FromArgb(value);

				yield return Instruction.Create(OpCodes.Ldc_R4, color.Red);
				yield return Instruction.Create(OpCodes.Ldc_R4, color.Green);
				yield return Instruction.Create(OpCodes.Ldc_R4, color.Blue);
				yield return Instruction.Create(OpCodes.Ldc_R4, color.Alpha);

				yield return Instruction.Create(OpCodes.Newobj, module.ImportCtorReference(("Microsoft.Maui.Graphics", "Microsoft.Maui.Graphics", "Color"), parameterTypes: new[] {
						("mscorlib", "System", "Single"),
						("mscorlib", "System", "Single"),
						("mscorlib", "System", "Single"),
						("mscorlib", "System", "Single")}));

				yield return Instruction.Create(OpCodes.Newobj, module.ImportCtorReference(("Microsoft.Maui.Controls", "Microsoft.Maui.Controls", "SolidColorBrush"), parameterTypes: new[] {
						("Microsoft.Maui.Graphics", "Microsoft.Maui.Graphics", "Color")}));

				yield break;
			}

			var parts = value.Split('.');
			if (parts.Length == 1 || (parts.Length == 2 && parts[0] == "Brush"))
			{
				var brush = parts[parts.Length - 1];
				var propertyGetterReference = module.ImportPropertyGetterReference(("Microsoft.Maui.Controls", "Microsoft.Maui.Controls", "Brush"),
																   brush,
																   isStatic: true,
																   caseSensitive: false);

				if (propertyGetterReference != null)
				{
					yield return Instruction.Create(OpCodes.Call, propertyGetterReference);
					yield break;
				}
			}

		} while (false);
		throw new BuildException(BuildExceptionCode.Conversion, node, null, value, typeof(Brush));
	}
}

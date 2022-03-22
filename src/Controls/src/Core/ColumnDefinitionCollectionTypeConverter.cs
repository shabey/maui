using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Microsoft.Maui.Controls.Xaml;

namespace Microsoft.Maui.Controls
{
	/// <include file="../../docs/Microsoft.Maui.Controls/ColumnDefinitionCollectionTypeConverter.xml" path="Type[@FullName='Microsoft.Maui.Controls.ColumnDefinitionCollectionTypeConverter']/Docs" />
	[ProvideCompiled("Microsoft.Maui.Controls.XamlC.ColumnDefinitionCollectionTypeConverter")]
	public class ColumnDefinitionCollectionTypeConverter : TypeConverter
	{
		/// <include file="../../docs/Microsoft.Maui.Controls/ColumnDefinitionCollectionTypeConverter.xml" path="//Member[@MemberName='CanConvertFrom']/Docs" />
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			=> sourceType == typeof(string);

		/// <include file="../../docs/Microsoft.Maui.Controls/ColumnDefinitionCollectionTypeConverter.xml" path="//Member[@MemberName='CanConvertTo']/Docs" />
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			=> destinationType == typeof(string);

		/// <include file="../../docs/Microsoft.Maui.Controls/ColumnDefinitionCollectionTypeConverter.xml" path="//Member[@MemberName='ConvertFrom']/Docs" />
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			var strValue = value?.ToString();

			if (strValue != null)
			{
				var lengths = strValue.Split(',');
				var converter = new GridLengthTypeConverter();
				return new ColumnDefinitionCollection(lengths.Select(length => new ColumnDefinition {Width = (GridLength)converter.ConvertFromInvariantString(length) }).ToArray());
			}

			throw new InvalidOperationException(string.Format("Cannot convert \"{0}\" into {1}", strValue, typeof(ColumnDefinitionCollection)));
		}


		/// <include file="../../docs/Microsoft.Maui.Controls/ColumnDefinitionCollectionTypeConverter.xml" path="//Member[@MemberName='ConvertTo']/Docs" />
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (value is not ColumnDefinitionCollection cdc)
				throw new NotSupportedException();
			var converter = new GridLengthTypeConverter();
			return string.Join(", ", cdc.Select(cd => converter.ConvertToInvariantString(cd.Width)));
		}
	}
}
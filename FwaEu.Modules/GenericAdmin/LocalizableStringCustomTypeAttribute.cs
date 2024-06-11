using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.GenericAdmin
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class LocalizableStringCustomTypeAttribute : Attribute, IPropertyAttribute
	{
		public const string CustomInnerTypeName = "LocalizableString";
		public string[] SupportedCultureCodes { get; set; }
		public void Enrich(Property property)
		{
			property.CustomInnerTypeName = CustomInnerTypeName;
			property.ExtendedProperties.Add(nameof(SupportedCultureCodes), SupportedCultureCodes);
		}
	}
}

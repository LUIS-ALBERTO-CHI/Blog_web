using FwaEu.Modules.GenericAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.GenericAdminMasterData
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public class EnumMasterDataAttribute : Attribute, IPropertyAttribute
	{
		public Type EnumType { get; }

		public EnumMasterDataAttribute(Type enumType)
		{
			this.EnumType = enumType ?? throw new ArgumentNullException(nameof(enumType));
		}

		public void Enrich(Property property)
		{
			property.ExtendedProperties.Add("MasterData", new { Name = this.EnumType.FullName });
		}
	}
}

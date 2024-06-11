using FwaEu.Modules.GenericAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.GenericAdminMasterData
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class MasterDataAttribute : Attribute, IPropertyAttribute
	{
		public MasterDataAttribute(string name)
		{
			this.Name = name ?? throw new ArgumentNullException(nameof(name));
		}

		public string Name { get; }

		public void Enrich(Property property)
		{
			property.ExtendedProperties.Add("MasterData", new { Name = this.Name });
		}
	}
}

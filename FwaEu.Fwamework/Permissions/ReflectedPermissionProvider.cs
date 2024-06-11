using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Permissions
{
	public abstract class ReflectedPermissionProvider : IPermissionProvider
	{
		public ReflectedPermissionProvider()
		{
			this.LoadProperties();
		}

		private void LoadProperties()
		{
			foreach (var property in GetPermissionProperties(this))
			{
				property.SetValue(this, new FromPropertyPermission(property));
			}
		}

		private static IEnumerable<PropertyDescriptor> GetPermissionProperties(object component)
		{
			return TypeDescriptor.GetProperties(component)
				.Cast<PropertyDescriptor>()
				.Where(p => p.PropertyType == typeof(IPermission));
		}

		public IEnumerable<IPermission> GetPermissions()
		{
			return GetPermissionProperties(this)
				.Select(p => (IPermission)p.GetValue(this));
		}
	}

	public class FromPropertyPermission : IPermission
	{
		public FromPropertyPermission(PropertyDescriptor property)
		{
			this.InvariantId = property.Name;
		}

		public string InvariantId { get; }
	}
}

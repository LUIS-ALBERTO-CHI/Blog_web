using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FwaEu.Modules.GenericImporter
{
	public interface IPropertyBinder
	{
		void SetValue(PropertyDescriptor property, object component, object value);
	}

	public class DefaultPropertyBinder : IPropertyBinder
	{
		private Dictionary<string, PropertyInfo> _propertyInfoByPropertyName;

		protected PropertyInfo GetPropertyInfo(object component, string propertyName)
		{
			if (this._propertyInfoByPropertyName == null)
			{
				this._propertyInfoByPropertyName = component.GetType()
					.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty)
					.ToDictionary(pi => pi.Name);
			}

			return this._propertyInfoByPropertyName[propertyName];
		}

		public void SetValue(PropertyDescriptor property, object component, object value)
		{
			if (property.IsReadOnly)
			{
				var propertyInfo = this.GetPropertyInfo(component, property.Name);
				propertyInfo.SetValue(component, value);
			}
			else
			{
				property.SetValue(component, value);
			}
		}
	}
}

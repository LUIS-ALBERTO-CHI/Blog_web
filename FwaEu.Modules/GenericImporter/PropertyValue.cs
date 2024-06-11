using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.GenericImporter
{
	public class PropertyValueSet
	{
		public PropertyValueSet(object value, string propertyName, Type propertyType)
			: this(propertyName, new[] { new PropertyValue(null, propertyType, value) })
		{
		}
		public PropertyValueSet(string propertyName, PropertyValue[] data)
		{
			this.PropertyName = (String.IsNullOrEmpty(propertyName) ? null : propertyName)
				?? throw new ArgumentNullException(nameof(propertyName));

			if (data == null || data.Length == 0)
			{
				throw new ArgumentNullException(nameof(data));
			}

			if (data.Length > 1 && !data.All(d => d.Name != null))
			{
				throw new ArgumentException(
					"All values of the set must have a sub property name.",
					nameof(data));
			}

			this.Data = data;
		}

		public string PropertyName { get; }
		public PropertyValue[] Data { get; }
	}

	public class PropertyValue
	{
		public PropertyValue(string name, Type type, object value)
		{
			this.Name = name;
			this.Type = type ?? throw new ArgumentNullException(nameof(type));
			this.Value = value;
		}

		public string Name { get; }
		public Type Type { get; }
		public object Value { get; }
	}
}

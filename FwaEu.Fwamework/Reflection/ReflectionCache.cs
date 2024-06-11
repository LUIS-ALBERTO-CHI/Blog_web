using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Reflection
{
	public class ReflectionCache
	{
		private readonly Dictionary<Type, Dictionary<string, Type>> 
			_propertyTypesByPropertyNameByModelType = new Dictionary<Type, Dictionary<string, Type>>();
		
		public Dictionary<string, Type> GetPropertyTypes(Type type)
		{		
			if (!_propertyTypesByPropertyNameByModelType.ContainsKey(type))
			{
				var newTypeProperties = TypeDescriptor.GetProperties(type)
					.Cast<PropertyDescriptor>()
					.ToDictionary(pd => pd.Name, pd => pd.PropertyType);
				_propertyTypesByPropertyNameByModelType.Add(type, newTypeProperties);
			}		
			return _propertyTypesByPropertyNameByModelType[type];
		}
	}
}

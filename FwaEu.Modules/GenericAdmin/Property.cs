using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FwaEu.Modules.GenericAdmin
{
	public interface IProperty
	{
		bool IsKey { get; set; }
		bool IsEditable { get; set; }
	}

	public class Property : IProperty
	{
		public static void Copy(IProperty from, IProperty to)
		{
			to.IsKey = from.IsKey;
			to.IsEditable = from.IsEditable;
		}

		public static Type SimplifyType(Type innerType)
		{
			var type = Nullable.GetUnderlyingType(innerType) ?? innerType;

			if (type.IsEnum)
			{
				type = typeof(string); //NOTE: Enum will be provided as the string value to client
			}

			return type;
		}

		public Property(string name, Type innerType)
		{
			this.Name = name;
			this.InnerType = innerType;
		}

		public bool IsKey { get; set;  }
		public string Name { get; }
		public Type InnerType { get; }
		public string CustomInnerTypeName { get; set; }

		public bool IsEditable { get; set; } = true;
		public Dictionary<string, object> ExtendedProperties { get; } = new Dictionary<string, object>();
	}
}
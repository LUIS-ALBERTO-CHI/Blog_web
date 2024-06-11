using System;
using System.Linq;

namespace FwaEu.Fwamework.Reflection
{
	public static class TypeHelper
	{
		private static Type[] NumericalWithDecimalPart = new[]
		{
			typeof(decimal),
			typeof(double),
			typeof(float),
		};

		public static bool IsNumericalWithDecimalPart(this Type type)
		{
			return NumericalWithDecimalPart.Contains(type);
		}

		public static bool AreEquivalent(Type t1, Type t2)
		{
			return t1 == t2 || t1.IsEquivalentTo(t2);
		}

		public static bool AreReferenceAssignable(Type dest, Type src)
		{
			return AreEquivalent(dest, src) || (!dest.IsValueType && !src.IsValueType && dest.IsAssignableFrom(src));
		}

		public static bool IsSimpleType(Type type)
		{
			return type.IsValueType || type.IsPrimitive ||
				new Type[]
				{
					typeof(string), typeof(decimal), typeof(DateTime),
					typeof(DateTimeOffset), typeof(TimeSpan), typeof(Guid)
				}.Contains(type) || Convert.GetTypeCode(type) != TypeCode.Object;
		}
	}
}

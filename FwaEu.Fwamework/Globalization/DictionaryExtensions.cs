using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Globalization
{
	public static class DictionaryExtensions
	{
		public static string ToStringFirstValue(this IDictionary dictionary)
		{
			var firstKey = dictionary.Keys.OfType<string>().FirstOrDefault();
			
			if (firstKey == null)
			{
				return null;
			}

			return (string)dictionary[firstKey];
		}
	}
}

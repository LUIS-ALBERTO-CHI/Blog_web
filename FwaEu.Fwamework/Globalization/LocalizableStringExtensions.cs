using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Globalization
{
	public static class LocalizableStringExtensions
	{
		public static Dictionary<string, string> ToStringStringDictionary(this IDictionary dictionary)
		{
			if (dictionary == null)
			{
				return null;
			}

			var stringStringDictionary = new Dictionary<string, string>();
			foreach (var key in dictionary.Keys)
			{
				stringStringDictionary[key.ToString()] = (string)dictionary[key];
			}
			return stringStringDictionary;
		}
	}
}

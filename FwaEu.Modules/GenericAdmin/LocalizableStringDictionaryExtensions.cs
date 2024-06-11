using FwaEu.Fwamework.Globalization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.GenericAdmin
{
	public static class LocalizableStringDictionaryExtensions
	{
		public static IDictionary ToLocalizableStringDictionary(this Dictionary<string, string> genericAdminDictionary)
		{
			var dictionary = (IDictionary)new LocalizableStringDictionary(); //HACK: Have to cast in IDictionary because DictionaryBase overrides [] implementation

			foreach (var kv in genericAdminDictionary)
			{
				dictionary[kv.Key] = kv.Value;
			}
			return dictionary;
		}
	}
}

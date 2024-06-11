using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FwaEu.Modules.GenericAdmin
{
	public static class Helper
	{
		public static Dictionary<string, object> CreateDictionary(IDictionary<string, object> from)
		{
			return from.ToDictionary(kv => kv.Key, kv => kv.Value);
		}
	}
}
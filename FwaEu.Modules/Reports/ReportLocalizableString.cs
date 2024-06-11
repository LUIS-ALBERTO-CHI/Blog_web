using FwaEu.Fwamework.Globalization;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Reports
{
	public class ReportLocalizableString : Dictionary<string, string>
	{
		public string GetString(string languageCode, string fallbackLanguageCode)
		{
			foreach (var code in new[] { languageCode, fallbackLanguageCode })
			{
				if (this.ContainsKey(code))
				{
					var value = this[code];
					if (!String.IsNullOrEmpty(value))
					{
						return value;
					}
				}
			}

			return this.ToStringFirstValue();
		}
	}
}

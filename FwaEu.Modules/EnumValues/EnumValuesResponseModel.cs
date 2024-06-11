using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.EnumValues
{
	public class EnumValuesGetModelsResult
	{
		public EnumValuesGetModelsResult(string enumTypeName, IEnumerable<string> values)
		{
			this.EnumTypeName = enumTypeName ?? throw new ArgumentNullException(nameof(enumTypeName));
			this.Values = values ?? throw new ArgumentNullException(nameof(values));
		}

		public string EnumTypeName { get; set; }
		public IEnumerable<string> Values { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FwaEu.Modules.EnumValues
{
	public interface IEnumValuesService
	{
		EnumValuesGetModelsResult GetEnumValues(string enumTypeName);
	}
}

using FwaEu.Modules.AssemblyProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.EnumValues
{
	public class DefaultEnumValuesService : IEnumValuesService
	{
		public DefaultEnumValuesService(IEnumerable<IAssemblyProvider> assemblyProviders)
		{
			this.Assemblies = assemblyProviders.Select(ap => ap.GetAssembly());
		}

		protected IEnumerable<Assembly> Assemblies { get; }

		public EnumValuesGetModelsResult GetEnumValues(string enumTypeName)
		{
			var enumType = Assemblies.Select(a => a.GetType(enumTypeName)).FirstOrDefault(et => et != null);

			if (enumType == null)
			{
				throw new EnumNotFoundException($"{enumTypeName} enum does not exist.");
			}

			var enumValues = Enum.GetValues(enumType).Cast<object>().Select(enumType => Convert.ToString(enumType));

			return new EnumValuesGetModelsResult(enumTypeName, enumValues);
		}
	}
}

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.EnumValues
{
	public static class EnumValuesExtensions
	{
		public static IServiceCollection AddFwameworkModuleEnumValues(this IServiceCollection services)
		{
			services.AddTransient<IEnumValuesService, DefaultEnumValuesService>();
			return services;
		}
	}
}

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.AssemblyProvider
{
	public static class AssemblyProviderExtensions
	{
		public static IServiceCollection AddFwameworkModuleAssemblyProvider(this IServiceCollection services)
		{
			services.AddTransient<IAssemblyProvider, FwaEuModulesAssemblyProvider>();
			return services;
		}
	}
}

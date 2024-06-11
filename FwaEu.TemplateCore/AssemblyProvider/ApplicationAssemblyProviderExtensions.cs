using FwaEu.Modules.AssemblyProvider;
using Microsoft.Extensions.DependencyInjection;

namespace FwaEu.TemplateCore.AssemblyProvider
{
	public static class ApplicationAssemblyProviderExtensions
	{
		public static IServiceCollection AddApplicationAssemblyProvider(this IServiceCollection services)
		{
			services.AddTransient<IAssemblyProvider, ApplicationAssemblyProvider>();
			return services;
		}
	}
}

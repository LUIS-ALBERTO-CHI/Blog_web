using FwaEu.Fwamework;
using FwaEu.Fwamework.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FwaEu.TemplateCore.Application
{
	public static class ApplicationInfoExtensions
	{
		public static IServiceCollection AddApplicationInformation(this IServiceCollection services,
			ApplicationInitializationContext context)
		{
			var section = context.Configuration.GetSection("Fwamework:ApplicationInfo");
			services.Configure<ApplicationInfoOptions>(section);
			services.AddSingleton<IApplicationInfo, DefaultApplicationInfo<Startup>>();
			return services;
		}
	}
}

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Configuration
{
	public static class ConfigurationExtensions
	{
		public static IServiceCollection AddFwameworkConfigurations(this IServiceCollection services)
		{
			services.AddTransient<IRelativePathProvider, ApplicationRootRelativePathProvider>();
			services.AddTransient<IRootPathProvider, DefaultRootPathProvider>();
			services.AddTransient<IPathFileProvider, DefaultPathFileProvider>();

			return services;
		}
	}
}

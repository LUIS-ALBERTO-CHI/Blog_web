using FwaEu.Fwamework.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.Application
{
	public static class RelativePathProvidersExtensions
	{
		public static IServiceCollection AddRelativePathProviders(
			this IServiceCollection services,
			IHostEnvironment hostEnvironment)
		{
			services.AddTransient<IRelativePathProvider>(sp =>
				new CodeBasedRelativePathProvider("IntegrationDirectory", "Integration"));

			if (hostEnvironment.IsDevelopment())
			{
				services.AddTransient<IRelativePathProvider>(sp =>
					new CodeBasedRelativePathProvider("DevelopmentDataDirectory", "DevelopmentData"));
				
				services.AddTransient<IRelativePathProvider>(sp =>
					new CodeBasedRelativePathProvider("DevelopmentTempDirectory", "_dev"));
			}

			return services;
		}
	}
}

using FwaEu.Fwamework.ProcessResults;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Imports
{
	public static class ImportsExtensions
	{
		public static IServiceCollection AddFwameworkImportServices(this IServiceCollection services)
		{
			services.AddTransient<IImportService, DefaultImportService>();
			services.AddTransient<IImportProcessResultFactory, DefaultImportProcessResultFactory>();

			return services;
		}
	}
}

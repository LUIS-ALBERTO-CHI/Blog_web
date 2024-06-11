using FwaEu.Fwamework;
using FwaEu.Modules.Reports;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.ReportsProvidersByFileSystem
{
	public static class ReportsProvidersByFileSystemExtensions
	{
		public static IServiceCollection AddFwameworkModuleReportsProvidersByFileSystem(
			this IServiceCollection services, ApplicationInitializationContext context)
		{
			var section = context.Configuration.GetSection("Fwamework:ReportsProvidersByFileSystem");
			services.Configure<ReportsProvidersByFileSystemOptions>(section);

			services.AddTransient<IReportProviderFactory, FileSystemReportProviderFactory>();
			services.AddTransient<FileSystemReportProvider>();

			return services;
		}
	}
}

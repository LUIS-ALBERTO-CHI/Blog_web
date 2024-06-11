using FwaEu.Modules.Reports;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.ReportsAdminProvidersByFileSystem
{
	public static class ReportsAdminProvidersByFileSystemExtensions
	{
		public static IServiceCollection AddFwameworkModuleReportsAdminProvidersByFileSystem(
			this IServiceCollection services)
		{
			services.AddTransient<IReportAdminProvider, ReportAdminProviderByFileSystem>();

			return services;
		}
	}
}

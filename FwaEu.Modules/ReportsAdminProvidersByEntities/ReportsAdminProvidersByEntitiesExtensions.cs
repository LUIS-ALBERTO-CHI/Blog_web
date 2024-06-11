using FwaEu.Modules.Reports;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.ReportsAdminProvidersByEntities
{
	public static class ReportsAdminProvidersByEntitiesExtensions
	{
		public static IServiceCollection AddFwameworkModuleReportsAdminProvidersByEntities(
			this IServiceCollection services)
		{
			services.AddTransient<IReportAdminProvider, ReportAdminProviderByEntities>();

			return services;
		}
	}
}


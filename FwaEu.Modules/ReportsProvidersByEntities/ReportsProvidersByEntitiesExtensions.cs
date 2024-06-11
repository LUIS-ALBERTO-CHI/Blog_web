using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Modules.Reports;
using FwaEu.Modules.ReportsAdminProvidersByEntities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.ReportsProvidersByEntities
{
	public static class ReportsProvidersByEntitiesExtensions
	{
		public static IServiceCollection AddFwameworkModuleReportsProvidersByEntities(
			this IServiceCollection services, ApplicationInitializationContext context)
		{
			var repositoryRegister = context.ServiceStore.Get<IRepositoryRegister>();
			repositoryRegister.Add<ReportEntityRepository>();

			services.AddTransient<IReportProviderFactory, EntitiesReportProviderFactory>();
			services.AddTransient<EntitiesReportProvider>();

			services.AddTransient<IReportAdminService, DefaultReportAdminService>();
			services.AddTransient<IReportAdminDataService, DefaultReportAdminDataService>();

			return services;
		}
	}
}

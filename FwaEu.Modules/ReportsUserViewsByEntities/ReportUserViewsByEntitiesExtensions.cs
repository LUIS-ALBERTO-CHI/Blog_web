using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Modules.Reports;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.ReportsUserViewsByEntities
{
	public static class ReportUserViewsByEntitiesExtensions
	{
		public static IServiceCollection AddFwameworkModuleReportsUserViewsByEntities(this IServiceCollection services,
			ApplicationInitializationContext context)
		{
			var repositoryRegister = context.ServiceStore.Get<IRepositoryRegister>();
			repositoryRegister.Add<ReportUserViewEntityRepository>();
			services.AddTransient<IReportUserViewService, DefaultReportUserViewService>();

			return services;
		}
	}
}

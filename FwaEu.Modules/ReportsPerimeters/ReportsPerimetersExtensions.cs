using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Modules.Reports;
using FwaEu.Modules.Users.UserPerimeter;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.ReportsPerimeters
{
	public static class ReportsPerimetersExtensions
	{
		public static IServiceCollection AddFwameworkModuleReportsPerimeters(this IServiceCollection services,
			ApplicationInitializationContext context)
		{
			var repositoryRegister = context.ServiceStore.Get<IRepositoryRegister>();
			repositoryRegister.Add<UserReportPerimeterEntityRepository>();

			services.AddTransient<IUserPerimeterProviderFactory, ReportPerimeterProviderFactory>();
			services.AddTransient<ReportPerimeterProvider>();

			services.AddTransient<IReportAccessService, EntityReportAccessService>();

			return services;
		}
	}
}

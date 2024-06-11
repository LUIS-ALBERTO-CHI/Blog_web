using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Permissions;
using FwaEu.Modules.ReportsProvidersByEntities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Reports
{
	public static class ReportsAdminExtensions
	{
		public static IServiceCollection AddFwameworkModuleReportsAdmin(this IServiceCollection services)
		{
			services.AddTransient<IPermissionProviderFactory, DefaultPermissionProviderFactory<ReportAdminPermissionProvider>>();

			return services;
		}
	}
}

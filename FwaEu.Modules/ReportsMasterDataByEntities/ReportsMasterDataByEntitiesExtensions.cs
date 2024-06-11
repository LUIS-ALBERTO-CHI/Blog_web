using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Modules.GenericAdmin;
using FwaEu.Modules.MasterData;
using FwaEu.Modules.Reports;
using FwaEu.Modules.ReportsMasterDataByEntities.GenericAdmin;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.ReportsMasterDataByEntities
{
	public static class ReportsMasterDataByEntitiesExtensions
	{
		public static IServiceCollection AddFwameworkModuleReportsMasterDataByEntities(this IServiceCollection services,
			ApplicationInitializationContext context)
		{
			var repositoryRegister = context.ServiceStore.Get<IRepositoryRegister>();
			repositoryRegister.Add<ReportCategoryEntityRepository>();
			repositoryRegister.Add<ReportFieldEntityRepository>();
			repositoryRegister.Add<ReportFilterEntityRepository>();

			services.AddMasterDataProvider<ReportCategoryMasterDataProvider>("ReportCategories");
			services.AddMasterDataProvider<ReportFilterMasterDataProvider>("ReportFilters");
			services.AddMasterDataProvider<ReportFieldMasterDataProvider>("ReportFields");

			services.AddGenericAdminConfiguration<ReportCategoryEntityToModelGenericAdminModelConfiguration>(ReportCategoryEntityToModelGenericAdminModelConfiguration.Key);
			services.AddGenericAdminConfiguration<ReportFieldEntityToModelGenericAdminModelConfiguration>(ReportFieldEntityToModelGenericAdminModelConfiguration.Key);
			services.AddGenericAdminConfiguration<ReportFilterEntityToModelGenericAdminModelConfiguration>(ReportFilterEntityToModelGenericAdminModelConfiguration.Key);

			services.AddTransient<IReportFilterDataProvider, MasterDataReportFilterDataProvider>();

			return services;
		}
	}
}

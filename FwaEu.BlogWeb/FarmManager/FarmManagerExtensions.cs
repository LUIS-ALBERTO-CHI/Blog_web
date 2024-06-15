using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Permissions;
using FwaEu.Fwamework.Users.Parts;
using FwaEu.Modules.Currencies.GenericAdmin;
using FwaEu.Modules.Currencies.MasterData;
using FwaEu.Modules.GenericAdmin;
using FwaEu.Modules.Logging;
using FwaEu.Modules.MasterData;
using FwaEu.Modules.Reports;
using FwaEu.Modules.Reports.WebApi;
using FwaEu.Modules.SearchEngine;
using FwaEu.Modules.SimpleMasterData;
using FwaEu.Modules.SimpleMasterData.GenericAdmin;
using FwaEu.Modules.SimpleMasterData.MasterData;
using FwaEu.Modules.Users.UserPerimeter;
using FwaEu.Modules.UserTasks;
using FwaEu.BlogWeb.FarmManager.Entities;
using FwaEu.BlogWeb.FarmManager.FarmerUserPart;
using FwaEu.BlogWeb.FarmManager.GenericAdmin;
using FwaEu.BlogWeb.FarmManager.MasterData;
using FwaEu.BlogWeb.FarmManager.Reports;
using FwaEu.BlogWeb.FarmManager.SearchEngine;
using FwaEu.BlogWeb.FarmManager.Services;
using FwaEu.BlogWeb.FarmManager.UserTasks;
using Microsoft.Extensions.DependencyInjection;


namespace FwaEu.BlogWeb.FarmManager
{
	public static class FarmManagerExtensions
	{
		public static IServiceCollection AddApplicationFarmManager(this IServiceCollection services,
				ApplicationInitializationContext context)
		{
			var repositoryRegister = context.ServiceStore.Get<IRepositoryRegister>();
			repositoryRegister.Add<FarmEntityRepository>();
			repositoryRegister.Add<FarmAnimalCountEntityRepository>();
			repositoryRegister.Add<TownRegionPerimeterEntityRepository>();
			repositoryRegister.Add<FarmPostalCodeEntityRepository>();

			services.For<FarmActivityEntity>(context)
				.AddRepository<FarmActivityEntityRepository>()
					.AddMasterDataProviderFactory()
					.AddGenericAdminModelConfiguration(options =>
						options.AddPermissionSecurityManager<FarmManagerPermissionProvider>(p => p.CanAdministrateFarmMasterData)
					);

			services.For<FarmAnimalSpeciesEntity>(context)
				.AddRepository<FarmAnimalSpeciesEntityRepository>()
					.AddMasterDataProviderFactory()
					.AddGenericAdminModelConfiguration(options =>
						options.AddPermissionSecurityManager<FarmManagerPermissionProvider>(p => p.CanAdministrateFarmMasterData)
					);

			services.For<FarmTownEntity>(context)
				.AddRepository<FarmTownEntityRepository>()
					.AddCustomMasterDataProviderFactory<FarmTownEntityMasterDataProvider>()
					.AddCustomGenericAdminModelConfiguration<FarmTownEntityToModelGenericAdminModelConfiguration>(options =>
						options.AddPermissionSecurityManager<FarmManagerPermissionProvider>(p => p.CanAdministrateFarmMasterData)
					);
			services.For<TownRegionEntity>(context)
				.AddRepository<TownRegionEntityRepository>()
				.AddCustomMasterDataProviderFactory<TownRegionEntityMasterDataProvider>();


			services.AddUserPerimeterProvider<TownRegionUserPerimeterProvider>(TownRegionPerimeterEntity.ProviderKey);

			services.AddGenericAdminConfiguration<FarmPostalCodeEntityToModelGenericAdminModelConfiguration>();

			services.AddMasterDataProvider<FarmPostalCodeMasterDataProvider>("PostalCodes");

			// NOTE: For GenericImporter, uncomment the following line to disable memory-cache for FarmTown
			// services.AddTransient<IDataAccessFactory<FarmTownEntity>, EntityDataAccessFactory<FarmTownEntity>>();

			services.AddTransient<IPermissionProviderFactory, DefaultPermissionProviderFactory<FarmManagerPermissionProvider>>();

			services.AddTransient<IFarmService, DefaultFarmService>();
			services.AddTransient<IFarmResponsibleService, DefaultFarmResponsibleService>();
			services.AddTransient<IAnimalCountService, DefaultAnimalCountService>();

			services.AddTransient<IReportDataProviderFactory, FarmInfosReportDataProviderFactory>();
			services.AddTransient<FarmInfosReportDataProvider>();
			services.AddTransient<IReportDataProvider>(sp => sp.GetRequiredService<FarmInfosReportDataProvider>());

			services.AddTransient<IListPartHandler, UserFarmerListPartHandler>();
			services.AddTransient<IPartHandler, UserFarmerPartHandler>();

			services.AddUserTask<FarmerCountUserTask>("FarmerCount");

			services.AddUserTask<FarmsWithoutAnimalsUserTask>("FarmsWithoutAnimals")
				.AddPermissionAccessManager<FarmManagerPermissionProvider>(p => p.CanAccessToFarmManager);

			services.AddSearchEngineResultProvider<FarmerSearchEngineResultProvider>("Farmer");
			services.AddSearchEngineResultProvider<FarmSearchEngineResultProvider>("Farm");
			services.AddSearchEngineResultProvider<FarmIdSearchEngineResultProvider>("FarmId");

			services.AddTransient<ILogEnricherProvider, FarmerLogEnricherProvider>();

			return services;
		}
	}
}

using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Permissions;
using FwaEu.Modules.Currencies.GenericAdmin;
using FwaEu.Modules.Currencies.MasterData;
using FwaEu.Modules.GenericAdmin;
using FwaEu.Modules.MasterData;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Currencies
{

	public static class CurrenciesExtensions
	{
		public static IServiceCollection AddFwameworkModuleCurrenciesServices(this IServiceCollection services,
			ApplicationInitializationContext context)
		{
			var repositoryRegister = context.ServiceStore.Get<IRepositoryRegister>();
			repositoryRegister.Add<CurrencyEntityRepository>();
			repositoryRegister.Add<ExchangeRateEntityRepository>();

			services.AddGenericAdminConfiguration<CurrencyEntityToModelGenericAdminModelConfiguration>(CurrencyEntityToModelGenericAdminModelConfiguration.Key);
			services.AddGenericAdminConfiguration<ExchangeRateEntityToModelGenericAdminModelConfiguration>(ExchangeRateEntityToModelGenericAdminModelConfiguration.Key);
			services.AddTransient<IPermissionProviderFactory, DefaultPermissionProviderFactory<CurrenciesPermissionProvider>>();

			services.AddMasterDataProvider<CurrencyMasterDataProvider>();
			services.AddMasterDataProvider<ExchangeRateMasterDataProvider>();

			return services;
		}
	}
}

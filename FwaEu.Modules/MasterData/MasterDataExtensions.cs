using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.Modules.SimpleMasterData.MasterData;
using Microsoft.Extensions.DependencyInjection;
using FwaEu.Modules.Authentication.MicrosoftIdentityPlatform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.MasterData
{
	public static class MasterDataExtensions
	{
		public static IServiceCollection AddFwameworkModuleMasterDataServices(this IServiceCollection services)
		{
			services.AddSingleton<MasterDataFactoryCache>();
			services.AddTransient<IMasterDataService, DefaultMasterDataService>();

			return services;
		}

		public static IMasterDataProviderInitializer AddMasterDataProvider<TProvider>(this IServiceCollection services, string key = null)
			where TProvider : class, IMasterDataProvider
		{
			var typeEntity = typeof(IEntity);
			var masterdataEntityType = typeof(TProvider).BaseType.GenericTypeArguments.FirstOrDefault(x => typeEntity.IsAssignableFrom(x));
			MasterDataProviderInitializer initializer = default;
			if (key == null)
			{
				var keyProviderType = typeof(IEntityKeyResolver<>).MakeGenericType(masterdataEntityType);
				services.AddTransient<IMasterDataProviderFactory>
				(
					sp => new InjectedMasterDataProviderFactory<TProvider>
					(
						((IEntityKeyResolver)sp.GetService(keyProviderType)).ResolveKey()
					)
				);
				initializer = new MasterDataProviderInitializer(sp => ((IEntityKeyResolver)sp.GetService(keyProviderType)).ResolveKey(), services);
			}
			else
			{
				services.AddTransient<IMasterDataProviderFactory>(sp => new InjectedMasterDataProviderFactory<TProvider>(key));
				initializer = new MasterDataProviderInitializer(sp => key, services);
			}

			services.AddTransient<TProvider>();
			if (masterdataEntityType != null)
			{
				initializer.AddRelatedEntity(masterdataEntityType);
			}
			return initializer;
		}

		public static IMasterDataProviderInitializer AddRelatedEntity<TEntity>(this IMasterDataProviderInitializer initializer)
		{
			initializer.AddRelatedEntity(typeof(TEntity));
			return initializer;
		}
		public static IMasterDataProviderInitializer AddRelatedEntity(this IMasterDataProviderInitializer initializer, Type entityType)
		{
			initializer.ServiceCollection.AddTransient<IMasterDataRelatedEntity>(provider =>
			new MasterDataRelatedEntity(initializer.ResolveKey(provider), entityType));
			return initializer;
		}
	}

}

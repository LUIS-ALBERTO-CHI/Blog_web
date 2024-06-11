using FwaEu.Fwamework.Data.Database;
using FwaEu.Modules.MasterData;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.SimpleMasterData.MasterData
{
	public static class SimpleMasterDataRegistrationExtensions
	{
		public static ISimpleMasterDataServiceInitializer AddCustomMasterDataProviderFactory<TProvider>(
			this ISimpleMasterDataServiceInitializer initializer)
			where TProvider : class, IMasterDataProvider
		{
			var type = typeof(SimpleMasterDataProviderFactory<,>)
				.MakeGenericType(initializer.EntityType, typeof(TProvider));

			initializer.ServiceCollection.AddTransient(typeof(IMasterDataProviderFactory), type);
			initializer.ServiceCollection.AddTransient<TProvider>();

			return initializer;
		}

		public static ISimpleMasterDataServiceInitializer AddDefaultCultureMasterDataProviderFactory(
			this ISimpleMasterDataServiceInitializer initializer)
		{
			var type = typeof(DefaultCultureSimpleMasterDataEntityProvider<,>)
				.MakeGenericType(initializer.RepositoryType, initializer.EntityType);

			initializer.InvokeMethod(typeof(SimpleMasterDataRegistrationExtensions),
				nameof(AddCustomMasterDataProviderFactory), new[] { type });

			return initializer;
		}

		public static ISimpleMasterDataServiceInitializer AddMasterDataProviderFactory(
			this ISimpleMasterDataServiceInitializer initializer)
		{
			var type = typeof(SimpleMasterDataEntityProvider<,>)
				.MakeGenericType(initializer.RepositoryType, initializer.EntityType);

			initializer.InvokeMethod(typeof(SimpleMasterDataRegistrationExtensions),
				nameof(AddCustomMasterDataProviderFactory), new[] { type });

			initializer.AddRelatedEntity();

			return initializer;
		}

		public static ISimpleMasterDataServiceInitializer AddRelatedEntity(this ISimpleMasterDataServiceInitializer initializer)
		{
			initializer.AddRelatedEntity(initializer.EntityType);
			return initializer;
		}
		public static ISimpleMasterDataServiceInitializer AddRelatedEntity<TEntity>(this ISimpleMasterDataServiceInitializer initializer)
		{
			initializer.AddRelatedEntity(typeof(TEntity));
			return initializer;
		}
		public static ISimpleMasterDataServiceInitializer AddRelatedEntity(this ISimpleMasterDataServiceInitializer initializer, Type entityType)
		{
			var keyProviderType = typeof(IEntityKeyResolver<>)
				.MakeGenericType(initializer.EntityType);

			initializer.ServiceCollection.AddTransient<IMasterDataRelatedEntity>
			(
				provider =>
					new MasterDataRelatedEntity(((IEntityKeyResolver)provider.GetService(keyProviderType)).ResolveKey(), entityType)
			);

			return initializer;
		}
	
	}
}

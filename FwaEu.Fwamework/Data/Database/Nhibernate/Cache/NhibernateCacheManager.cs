using FwaEu.Fwamework.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Data.Database.Nhibernate.Cache
{
	public class NhibernateCacheManager : ICacheManager
	{
		public NhibernateCacheManager(IEnumerable<INhibernateConfigurationLoader> nhibernateConfigurationLoaders)
		{
			this._nhibernateConfigurationLoaders = nhibernateConfigurationLoaders;
		}

		private readonly IEnumerable<INhibernateConfigurationLoader> _nhibernateConfigurationLoaders;

		public async Task ClearAsync()
		{
			foreach (var loader in this._nhibernateConfigurationLoaders)
			{
				var configuration = loader.Load();
				var sessionFactory = (NHibernate.Impl.SessionFactoryImpl)configuration.BuildSessionFactory();

				//Evict default cache region
				await sessionFactory.EvictQueriesAsync();

				//Evict named cache regions
				var allCacheRegions = sessionFactory.GetAllSecondLevelCacheRegions();
				foreach (var region in allCacheRegions)
				{
					await sessionFactory.EvictQueriesAsync(region.Key);
				}

				foreach (var collectionMetadata in sessionFactory.GetAllCollectionMetadata())
				{
					await sessionFactory.EvictCollectionAsync(collectionMetadata.Key);
				}

				foreach (var classMetadata in sessionFactory.GetAllClassMetadata())
				{
					await sessionFactory.EvictEntityAsync(classMetadata.Key);
				}
			}
		}
	}
}

using NHibernate.Cfg.Loquacious;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Data.Database.Nhibernate
{
	public class CodeBasedNhibernateConfigurationLoader : NhibernateConfigurationLoaderBase
	{
		private readonly Action<IDbIntegrationConfigurationProperties> _configureDatabaseProperties;
		private readonly Action<NHibernate.Cfg.Configuration> _finalizeConfiguration;

		public CodeBasedNhibernateConfigurationLoader(IServiceProvider serviceProvider,
			string connectionStringName,
			Type databaseFeaturesType,
			IEnumerable<Type> typesInWhichSearchForMappings,
			Action<IDbIntegrationConfigurationProperties> configureDatabaseProperties,
			Action<NHibernate.Cfg.Configuration> finalizeConfiguration = null)
			: base(serviceProvider, typesInWhichSearchForMappings)
		{
			ConnectionStringName = connectionStringName ?? throw new ArgumentNullException(nameof(connectionStringName));
			DatabaseFeaturesType = databaseFeaturesType ?? throw new ArgumentNullException(nameof(databaseFeaturesType));
			_configureDatabaseProperties = configureDatabaseProperties ?? throw new ArgumentNullException(nameof(configureDatabaseProperties));
			_finalizeConfiguration = finalizeConfiguration;
		}

		public override string ConnectionStringName { get; }

		public override Type DatabaseFeaturesType { get; }

		protected override void ConfigureDatabaseProperties(IDbIntegrationConfigurationProperties properties)
		{
			_configureDatabaseProperties(properties);
		}

		protected override void FinalizeConfiguration(NHibernate.Cfg.Configuration configuration)
		{
			base.FinalizeConfiguration(configuration);

			configuration.Cache(options =>
			{
				options.Provider<NHibernate.Caches.CoreMemoryCache.CoreMemoryCacheProvider>();
				options.UseQueryCache = true;
			});

			_finalizeConfiguration?.Invoke(configuration);
		}
	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Mapping;
using FluentNHibernate.Mapping.Providers;
using FwaEu.Fwamework.DependencyInjection;
using FwaEu.Fwamework.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Cfg;
using NHibernate.Cfg.Loquacious;

namespace FwaEu.Fwamework.Data.Database.Nhibernate
{
	public interface INhibernateConfigurationLoader
	{
		string ConnectionStringName { get; }
		Type DatabaseFeaturesType { get; }
		NHibernate.Cfg.Configuration Load();
	}

	public abstract class NhibernateConfigurationLoaderBase : INhibernateConfigurationLoader
	{
		protected NhibernateConfigurationLoaderBase(
			IServiceProvider serviceProvider,
			IEnumerable<Type> typesInWhichSearchForMappings)
		{
			this.ServiceProvider = serviceProvider
				?? throw new ArgumentNullException(nameof(serviceProvider));

			this._typesInWhichSearchForMappings = typesInWhichSearchForMappings
				?? throw new ArgumentNullException(nameof(typesInWhichSearchForMappings));
		}

		public abstract string ConnectionStringName { get; }
		public abstract Type DatabaseFeaturesType { get; }

		protected IServiceProvider ServiceProvider { get; }
		private readonly IEnumerable<Type> _typesInWhichSearchForMappings;
		protected abstract void ConfigureDatabaseProperties(IDbIntegrationConfigurationProperties configurationProperties);

		protected virtual void FinalizeConfiguration(NHibernate.Cfg.Configuration configuration)
		{

		}

		public NHibernate.Cfg.Configuration Load()
		{
			var configuration = new NHibernate.Cfg.Configuration();

			configuration.DataBaseIntegration((properties) =>
			{
				this.ConfigureDatabaseProperties(properties);
			});
			var fluentConfiguration = Fluently.Configure(configuration);
			fluentConfiguration.Mappings(mappingConfig =>
			{
				foreach (var mappingType in FilterMappings(this._typesInWhichSearchForMappings))
				{
					mappingConfig.FluentMappings.Add(mappingType);
				}

				var featuresProvider = this.ServiceProvider.GetServices<IDatabaseFeaturesProvider>()
					.First(fp => fp.DatabaseFeaturesType == this.DatabaseFeaturesType);

				var pluralizationService = this.ServiceProvider.GetRequiredService<IPluralizationService>();

				var settings = new FwaConventions.FwaConventionsOptions(featuresProvider.GetDatabaseFeatures());
				var fwaConventions = new FwaConventions(settings, pluralizationService);

				mappingConfig.FluentMappings.Conventions.Add(fwaConventions);
				mappingConfig.FluentMappings.Conventions.Add(new FwaManyToManyTableNameConvention(fwaConventions));
			});

			configuration = fluentConfiguration.BuildConfiguration();

			this.FinalizeConfiguration(configuration);

			//NOTE: Nhibernate cannot currently load the connection string by its name on .NET Core
			if (!configuration.Properties.ContainsKey(NHibernate.Cfg.Environment.ConnectionString))
				configuration.Properties.Add(NHibernate.Cfg.Environment.ConnectionString, this.ServiceProvider.GetRequiredService<IConfiguration>()
						.GetConnectionString(this.ConnectionStringName));

			var scopedServiceProvider = this.ServiceProvider.GetRequiredService<IScopedServiceProvider>();
			foreach (var eventListenersInitializer in this.ServiceProvider.GetService<IEnumerable<IEventListenersInitializer>>())
			{
				eventListenersInitializer.Initialize(configuration.EventListeners, scopedServiceProvider);
			}

			return configuration;
		}

		private static IEnumerable<Type> FilterMappings(IEnumerable<Type> types)
		{
			//NOTE: Taken from AddMappingsFromSource: https://github.com/FluentNHibernate/fluent-nhibernate/blob/161ffa1fef09306126692760b1c5a194b8d5df01/src/FluentNHibernate/PersistenceModel.cs
			return types.Where(type =>
				IsMappingOf<IMappingProvider>(type) ||
				IsMappingOf<IIndeterminateSubclassMappingProvider>(type) ||
				IsMappingOf<IExternalComponentMappingProvider>(type) ||
				IsMappingOf<IFilterDefinition>(type));
		}

		private static bool IsMappingOf<T>(Type type)
		{
			return !type.IsGenericType && typeof(T).IsAssignableFrom(type);
		}
	}
}

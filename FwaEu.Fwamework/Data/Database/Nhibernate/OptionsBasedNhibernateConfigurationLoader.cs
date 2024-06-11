using NHibernate.Cfg.Loquacious;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Data.Database.Nhibernate
{
	public class OptionsBasedNhibernateConfigurationLoader : NhibernateConfigurationLoaderBase
	{
		private readonly NhibernateConfigurationOptions _options;

		public OptionsBasedNhibernateConfigurationLoader(IServiceProvider serviceProvider,
			NhibernateConfigurationOptions options,
			IEnumerable<Type> typesInWhichSearchForMappings)
			: base(serviceProvider, typesInWhichSearchForMappings)
		{
			_options = options;
			DatabaseFeaturesType = Type.GetType(_options.DatabaseFeaturesTypeFullName);
		}

		public override string ConnectionStringName => _options.Properties[NHibernate.Cfg.Environment.ConnectionStringName];

		public override Type DatabaseFeaturesType { get; }

		protected override void ConfigureDatabaseProperties(IDbIntegrationConfigurationProperties configurationProperties)
		{
			//NOTE: All properties are set on FinalizeConfiguration
		}

		protected override void FinalizeConfiguration(NHibernate.Cfg.Configuration configuration)
		{
			base.FinalizeConfiguration(configuration);
			configuration.AddProperties(this._options.Properties);
		}
	}
}

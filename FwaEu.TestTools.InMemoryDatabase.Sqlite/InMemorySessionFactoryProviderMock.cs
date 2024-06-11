using FluentNHibernate.Cfg;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Nhibernate;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Text;
using FwaEu.Modules.Data.Database.SQLite;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace FwaEu.TestTools.InMemoryDatabase.Sqlite
{
	public class InMemorySessionFactoryProviderMock : ISessionFactoryProvider
	{
		public const string MemorySharedConnectionString = "FullUri=file:memorydb.db?mode=memory&cache=shared";

		private readonly InMemoryDatabaseCreationOptions _options;
		private readonly SQLiteConnection _connection;

		public InMemorySessionFactoryProviderMock(
			InMemoryDatabaseCreationOptions options,
			SQLiteConnection connection)
		{
			this._options = options
				?? throw new ArgumentNullException(nameof(options));

			this._connection = connection
				?? throw new ArgumentNullException(nameof(connection));
		}

		private ISessionFactory _dataScopeSessionFactory;
		public DefaultConnectionInfo DefaultConnectionInfo => throw new NotImplementedException();

		public ISessionFactory GetFactory(CreateSessionOptions options)
		{
			if (this._dataScopeSessionFactory == null)
			{
				var configuration = new NHibernate.Cfg.Configuration();

				configuration.DataBaseIntegration((properties) =>
				{
					properties.Dialect<SQLiteDialect>();
					properties.ConnectionString = MemorySharedConnectionString;
					properties.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
					properties.LogFormattedSql = true;
					properties.LogSqlInConsole = false;
					properties.ConnectionReleaseMode = ConnectionReleaseMode.OnClose;
				});

				var fluentConfiguration = Fluently.Configure(configuration);
				fluentConfiguration.Mappings(mappingConfig =>
				{
					foreach (var mappingType in this._options.MappingTypes)
					{
						mappingConfig.FluentMappings.Add(mappingType);
					}

					var fwaConventions = new FwaConventions(
						new FwaConventions.FwaConventionsOptions(
							new SQLiteDatabaseFeatures()),
							new PluralizeNetCorePluralizationService()
						);

					mappingConfig.FluentMappings.Conventions.Add(fwaConventions);
					mappingConfig.FluentMappings.Conventions.Add(new FwaManyToManyTableNameConvention(fwaConventions));
				});

				configuration = fluentConfiguration.BuildConfiguration();

				if (this._connection.State != System.Data.ConnectionState.Open)
				{
					this._connection.Open();
				}

				var schemaExport = new SchemaExport(configuration);
				schemaExport.Execute(false, true, false, this._connection, null);

				this._dataScopeSessionFactory = configuration.BuildSessionFactory();
			}

			return this._dataScopeSessionFactory;
		}
	}
}

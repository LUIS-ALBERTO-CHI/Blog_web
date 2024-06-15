using FwaEu.Fwamework;
using FwaEu.Fwamework.Authentication;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Nhibernate;
using FwaEu.Fwamework.Users;
using FwaEu.BlogWeb.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Cfg;
using NHibernate.Dialect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FwameworkDataBaseExtensions = FwaEu.Fwamework.Data.Database.DatabaseExtensions;

namespace FwaEu.BlogWeb.Initialization
{
	public static class DatabaseExtensions
	{
		private static IEnumerable<Type> GetTypesInWhichSearchForMappings(IAuthenticationFeatures authenticationFeatures)
		{
			return authenticationFeatures.CustomizeTypesInWhichSearchForMappings(
											typeof(FwameworkDataBaseExtensions).Assembly.GetExportedTypes()
												.Concat(typeof(FwaEu.Modules.Data.Database.MsSql.MsSqlDatabaseFeatures).Assembly.GetExportedTypes()
													.Concat(typeof(DatabaseExtensions).Assembly.GetExportedTypes())));
		}

		private static IEnumerable<INhibernateConfigurationLoader> GetLoaders(
			IServiceProvider serviceProvider,
			IAuthenticationFeatures authenticationFeatures)
		{
			var section = serviceProvider.GetService<IConfiguration>().GetSection("Application:Nhibernate");
			var settings = section.Get<SimpleNhibernateOptions>() ?? new SimpleNhibernateOptions();

			//NOTE: If you want to load the configuration from appSettings, you can use the following syntax
			//("configurationOptions" can be loaded using the NhibernateConfigurationOptions class)

			//yield return new OptionsBasedNhibernateConfigurationLoader(
			//   serviceProvider: serviceProvider,
			//   options: configurationOptions,
			//   typesInWhichSearchForMappings: GetTypesInWhichSearchForMappings(authenticationFeatures));

			//NOTE: For most cases, the CodeBasedNhibernateConfigurationLoader will be enough

			var sectionConnectionStrings = serviceProvider.GetService<IConfiguration>().GetSection("ConnectionStrings");
			var settingsConnectionStrings = sectionConnectionStrings.Get<Dictionary<string, string>>();

			//NOTE: Get all types from all projects (Fwamwork + Modules + Current assembly)
			var typesFromAllAssemblies = GetTypesInWhichSearchForMappings(authenticationFeatures);

			foreach (var connectionString in settingsConnectionStrings)
			{
				var keyofConnectionString = connectionString.Key;
				var specificTypesForMappings = typesFromAllAssemblies
												.Where(x => (!x.IsDefined(typeof(ConnectionStringAttribute))
																&& keyofConnectionString.Equals("Default", StringComparison.InvariantCultureIgnoreCase))
															|| (x.IsDefined(typeof(ConnectionStringAttribute))
																&& ((ConnectionStringAttribute)Attribute.GetCustomAttribute(x,
																	typeof(ConnectionStringAttribute)))?.Name == keyofConnectionString));
				yield return new CodeBasedNhibernateConfigurationLoader(
					serviceProvider: serviceProvider,
					connectionStringName: keyofConnectionString,
					databaseFeaturesType: typeof(FwaEu.Modules.Data.Database.MsSql.MsSqlDatabaseFeatures),
					typesInWhichSearchForMappings: specificTypesForMappings,
					configureDatabaseProperties: (properties) =>
					{
						properties.Dialect<MsSql2012Dialect>();
						properties.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
						properties.LogFormattedSql = settings.LogFormattedSql;
						properties.LogSqlInConsole = settings.LogSqlInConsole;
					},
					finalizeConfiguration: (configuration) =>
					{
						configuration.Properties[NHibernate.Cfg.Environment.DefaultFlushMode] = NHibernate.FlushMode.Manual.ToString();
					}
				);
			}
		}

		private static void AddDatabaseFeatures(this IServiceCollection services)
		{
			//NOTE: If you want to dynamically register your>s< database>s< features, you can use the following syntax
			//that allows you to load the "databaseFeaturesFullTypeName" variable value from appSettings for example

			//services.AddTransient<IDatabaseFeaturesProvider>(sp =>
			//	new DatabaseFeaturesProvider(databaseFeaturesFullTypeName));

			//NOTE: You can also enable the database>s< you need to use in your application as follows

			services.AddTransient<IDatabaseFeaturesProvider>(sp =>
				new DatabaseFeaturesProvider<Modules.Data.Database.MsSql.MsSqlDatabaseFeatures>());

			//services.AddTransient<IDatabaseFeaturesProvider>(sp =>
			//	new DatabaseFeaturesProvider<Modules.Data.Database.MySql.MySqlDatabaseFeatures>());

			//services.AddTransient<IDatabaseFeaturesProvider>(sp =>
			//	new DatabaseFeaturesProvider<Modules.Data.Database.Oracle.OracleDatabaseFeatures>());

			//services.AddTransient<IDatabaseFeaturesProvider>(sp =>
			//	new DatabaseFeaturesProvider<Modules.Data.Database.SQLite.SQLiteDatabaseFeatures>());
		}

		public static IServiceCollection AddApplicationDatabase(
			this IServiceCollection services,
			ApplicationInitializationContext context,
			IAuthenticationFeatures authenticationFeatures)
		{
			var repositories = context.ServiceStore.Get<IRepositoryRegister>();
			repositories.Add<UserEntity, ApplicationUserEntity, IUserEntityRepository, ApplicationUserEntityRepository>();

			services.AddSingleton(repositories);
			services.AddDatabaseFeatures();
			services.AddTransient((serviceProvider) => GetLoaders(serviceProvider, authenticationFeatures));

			return services;
		}
	}
}

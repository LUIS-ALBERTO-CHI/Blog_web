using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Nhibernate;
using FwaEu.Fwamework.ProcessResults;
using FwaEu.Fwamework.Setup;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Data.Database
{
	public class ConnectionStringsSetupTask : ISetupTask
	{
		public string Name => "ConnectionStrings";
		private const string maskedPassword = "*****";
		public Type ArgumentsType => null;
		private readonly IEnumerable<INhibernateConfigurationLoader> _nhibernateConfigurationLoaders;
		private readonly IConfiguration _configuration;
		private readonly IEnumerable<IDatabaseFeaturesProvider> _databaseFeaturesProviders;
		public ConnectionStringsSetupTask(IEnumerable<INhibernateConfigurationLoader> nhibernateConfigurationLoaders,
			IEnumerable<IDatabaseFeaturesProvider> databaseFeaturesProviders, IConfiguration configuration)
		{
			this._nhibernateConfigurationLoaders = nhibernateConfigurationLoaders ?? throw new ArgumentNullException(nameof(nhibernateConfigurationLoaders));
			this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			this._databaseFeaturesProviders = databaseFeaturesProviders ?? throw new ArgumentNullException(nameof(databaseFeaturesProviders));
		}
		public Task<ISetupTaskResult> ExecuteAsync(object arguments)
		{
			var result = new ProcessResult();
			var connectionStrings = this._nhibernateConfigurationLoaders.Select(loader =>
			{
				var connectionString = _configuration.GetConnectionString(loader.ConnectionStringName);
				var provider = this._databaseFeaturesProviders.First(x => x.DatabaseFeaturesType == loader.DatabaseFeaturesType);
				var passwordKeys = provider.GetDatabaseFeatures().ConnectionStringPasswordKeys;
				string[] connectionSubStrings = connectionString.Split(";");
				for (int i = 0; i < connectionSubStrings.Length; i++)
				{
					var splitValue = connectionSubStrings[i].Trim().Split("=");
					if (splitValue.Length == 2 && passwordKeys.Contains(splitValue[0].Trim(), StringComparer.InvariantCultureIgnoreCase))
					{
						splitValue[1] = maskedPassword;
						connectionSubStrings[i] = string.Join("=", splitValue);
					}
				}
				return new ConnectionStringModel
				{
					ConnectionString = string.Join(";", connectionSubStrings),
					Name = loader.ConnectionStringName
				};
			}).ToArray();

			return Task.FromResult<ISetupTaskResult>(new SetupTaskResult<ConnectionStringModel[]>(
						new ProcessResult(),
						connectionStrings));
		}
	}

	public class ConnectionStringModel
	{
		public string Name { get; set; }
		public string ConnectionString { get; set; }
	}
}

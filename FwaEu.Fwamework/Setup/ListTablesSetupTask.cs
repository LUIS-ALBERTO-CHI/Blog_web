using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Nhibernate;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.ProcessResults;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Setup
{
	public class ListTablesSetupTask : ISetupTask
	{
		public string Name => "ListTables";
		public Type ArgumentsType => null;

		private readonly IEnumerable<INhibernateConfigurationLoader> _nhibernateConfigurationLoaders;
		private readonly IEnumerable<IDatabaseFeaturesProvider> _databaseFeaturesProviders;
		private readonly ISessionAdapterFactory _sessionAdapterFactory;

		public ListTablesSetupTask(
			IEnumerable<INhibernateConfigurationLoader> nhibernateConfigurationLoaders,
			IEnumerable<IDatabaseFeaturesProvider> databaseFeaturesProviders,
			ISessionAdapterFactory sessionAdapterFactory)
		{
			this._nhibernateConfigurationLoaders = nhibernateConfigurationLoaders
				?? throw new ArgumentNullException(nameof(nhibernateConfigurationLoaders));

			this._databaseFeaturesProviders = databaseFeaturesProviders
				?? throw new ArgumentNullException(nameof(databaseFeaturesProviders));

			this._sessionAdapterFactory = sessionAdapterFactory
				?? throw new ArgumentNullException(nameof(sessionAdapterFactory));
		}
		public async Task<ISetupTaskResult> ExecuteAsync(object arguments)
		{
			var processResult = new ProcessResult();
			var tablesByDatabase = new Dictionary<string, string[]>();

			foreach (var loader in this._nhibernateConfigurationLoaders)
			{
				var provider = this._databaseFeaturesProviders.First(x => x.DatabaseFeaturesType == loader.DatabaseFeaturesType);

				using (var session = (INhibernateStatefulSessionAdapter)_sessionAdapterFactory
					.CreateStatefulSession(new CreateSessionOptions { ConnectionStringName = loader.ConnectionStringName }))
				{
					var tablesList = await session.NhibernateSession
						.CreateSQLQuery(provider.GetDatabaseFeatures().GetAllTablesSql)
						.ListAsync<string>();

					tablesByDatabase.Add(loader.ConnectionStringName, tablesList.ToArray());
				}
			}

			return new SetupTaskResult<TablesListModel>(processResult, new TablesListModel(tablesByDatabase));
		}
	}

	public class TablesListModel
	{
		public TablesListModel(Dictionary<string, string[]> tablesByDatabase)
		{
			TablesByDatabase = tablesByDatabase ?? throw new ArgumentNullException(nameof(tablesByDatabase));
		}

		public Dictionary<string, string[]> TablesByDatabase { get; }
	}
}

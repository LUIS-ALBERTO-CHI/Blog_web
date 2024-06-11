using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Nhibernate;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.ProcessResults;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Setup
{
	public class DeleteTablesSetupTask : SetupTask<DeleteTablesParameters>
	{
		public override string Name => "DeleteTables";
		private readonly IEnumerable<INhibernateConfigurationLoader> _nhibernateConfigurationLoaders;
		private readonly ISessionAdapterFactory _sessionAdapterFactory;

		public DeleteTablesSetupTask(IEnumerable<INhibernateConfigurationLoader> nhibernateConfigurationLoaders,
			ISessionAdapterFactory sessionAdapterFactory)
		{
			_nhibernateConfigurationLoaders = nhibernateConfigurationLoaders
				?? throw new ArgumentNullException(nameof(nhibernateConfigurationLoaders));
			this._sessionAdapterFactory = sessionAdapterFactory
				?? throw new ArgumentNullException(nameof(sessionAdapterFactory));
		}

		public override async Task<NoDataSetupTaskResult> ExecuteAsync(DeleteTablesParameters arguments)
		{
			var result = new ProcessResult();
			var context = result.CreateContext($"Delete tables from databases", "DeleteTablesSetupTask");
			var invariantTablesByDatabase = new Dictionary<string, string[]>(arguments.TablesByDatabase, StringComparer.InvariantCultureIgnoreCase);

			foreach (var loader in this._nhibernateConfigurationLoaders)
			{
				var tablesToDelete = invariantTablesByDatabase[loader.ConnectionStringName];

				using (var session = (INhibernateStatefulSessionAdapter)_sessionAdapterFactory.CreateStatefulSession())
				{
					foreach (var table in tablesToDelete)
					{
						try
						{
							var query = string.Format("DROP TABLE {0};", table);
							context.Add(new SqlProcessResultEntry(query));

							await session.NhibernateSession
							.CreateSQLQuery(query)
							.ExecuteUpdateAsync();
							context.Add(new InfoProcessResultEntry(string.Format("Removed table '{0}' in database '{1}'",
							table, loader.ConnectionStringName)));

						}
						catch (Exception ex)
						{
							context.Add(ErrorProcessResultEntry.FromException(ex));
						}
					}
				}
			}

			return new NoDataSetupTaskResult(result);
	}
}

public class DeleteTablesParameters
{
	public DeleteTablesParameters(Dictionary<string, string[]> tablesByDatabase)
	{
		this.TablesByDatabase = tablesByDatabase ?? throw new ArgumentNullException(nameof(tablesByDatabase));
	}

	public Dictionary<string, string[]> TablesByDatabase { get; }
}
}

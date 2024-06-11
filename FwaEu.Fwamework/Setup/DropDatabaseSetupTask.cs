using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Nhibernate;
using FwaEu.Fwamework.ProcessResults;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Setup
{
	public class DropDatabaseSetupTask : SchemaSetupTask
	{
		public DropDatabaseSetupTask(IEnumerable<INhibernateConfigurationLoader> nhibernateConfigurationLoaders)
		{
			this._nhibernateConfigurationLoaders = nhibernateConfigurationLoaders;
		}

		public override string Name => "DropDatabase";

		protected override async Task ApplySchemaChangeAsync(ProcessResultContext context,
			INhibernateConfigurationLoader loader, SchemaParameters arguments)
		{
			var configuration = loader.Load();
			var schemaExport = new SchemaExport(configuration);

			await schemaExport.ExecuteAsync(scriptAction => context.Add(new SqlProcessResultEntry(scriptAction)),
					arguments.Action == SchemaAction.ApplyChangesOnDatabase, arguments.Action == SchemaAction.ApplyChangesOnDatabase);
		}

		protected override ProcessResultContext CreateProcessResultContext(ProcessResult result,
			INhibernateConfigurationLoader loader, SchemaParameters arguments)
		{
			return result.CreateContext(
					$"Drop schema of database '{loader.ConnectionStringName}', action '{arguments.Action}'",
					"DropDatabase");
		}
	}
}

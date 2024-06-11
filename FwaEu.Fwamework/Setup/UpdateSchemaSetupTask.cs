using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Nhibernate;
using FwaEu.Fwamework.ProcessResults;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Setup
{
	public enum SchemaAction
	{
		GenerateSql,
		ApplyChangesOnDatabase
	}

	public class SchemaParameters
	{
		public SchemaParameters(SchemaAction? action)
		{
			this.Action = action;
		}

		[Required]
		public SchemaAction? Action { get; }
	}

	public class UpdateSchemaSetupTask : SchemaSetupTask
	{
		public UpdateSchemaSetupTask(IEnumerable<INhibernateConfigurationLoader> nhibernateConfigurationLoaders)
		{
			this._nhibernateConfigurationLoaders = nhibernateConfigurationLoaders;
		}

		public override string Name => "UpdateSchema";

		protected override ProcessResultContext CreateProcessResultContext(ProcessResult result,
			INhibernateConfigurationLoader loader, SchemaParameters arguments)
		{
			return result.CreateContext(
					$"Update schema of database '{loader.ConnectionStringName}', action '{arguments.Action}'",
					"UpdateSchemaSetupTask");
		}

		protected override async Task ApplySchemaChangeAsync(ProcessResultContext context,
			INhibernateConfigurationLoader loader, SchemaParameters arguments)
		{
			var configuration = loader.Load();
			var schemaUpdate = new SchemaUpdate(configuration);

			await schemaUpdate.ExecuteAsync(
					scriptAction => context.Add(new SqlProcessResultEntry(scriptAction)),
					arguments.Action == SchemaAction.ApplyChangesOnDatabase);

			context.Add(schemaUpdate.Exceptions
				.Select(ErrorProcessResultEntry.FromException));
		}
	}

	public abstract class SchemaSetupTask : SetupTask<SchemaParameters>
	{
		protected abstract ProcessResultContext CreateProcessResultContext(ProcessResult result,
			INhibernateConfigurationLoader loader, SchemaParameters arguments);
		protected abstract Task ApplySchemaChangeAsync(ProcessResultContext context,
			INhibernateConfigurationLoader loader, SchemaParameters arguments);

		protected IEnumerable<INhibernateConfigurationLoader> _nhibernateConfigurationLoaders;

		public override async Task<NoDataSetupTaskResult> ExecuteAsync(SchemaParameters arguments)
		{
			var result = new ProcessResult();

			foreach (var loader in this._nhibernateConfigurationLoaders)
			{
				var context = this.CreateProcessResultContext(result, loader, arguments);

				await this.ApplySchemaChangeAsync(context, loader, arguments);
			}

			return new NoDataSetupTaskResult(result);
		}
	}
}

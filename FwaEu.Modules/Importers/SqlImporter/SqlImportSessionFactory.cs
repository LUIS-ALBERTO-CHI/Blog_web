using FwaEu.Fwamework.Imports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using FwaEu.Fwamework.Data.Database.Nhibernate;

namespace FwaEu.Modules.Importers.SqlImporter
{
	public class SqlImportSessionFactory : IImportSessionFactory
	{
		public SqlImportSessionFactory(IServiceProvider serviceProvider)
		{
			this._serviceProvider = serviceProvider;
		}

		private readonly IServiceProvider _serviceProvider;

		private SqlImporter CreateSqlImporter()
		{
			var nhibernateSessionAdapterFactory = this._serviceProvider.GetRequiredService<INhibernateSessionAdapterFactory>();
			return new SqlImporter(nhibernateSessionAdapterFactory);
		}

		public Task<IImportFileSession> CreateImportSessionAsync(ImportContext context, IImportFile[] files)
		{
			var sqlFiles = files.Where(f => f.Name.EndsWith(".sql", StringComparison.InvariantCultureIgnoreCase)).ToArray();

			return Task.FromResult(sqlFiles.Any()
				? new SqlImportFileSession(this.CreateSqlImporter(), context, sqlFiles)
				: default(IImportFileSession));
		}
	}
}

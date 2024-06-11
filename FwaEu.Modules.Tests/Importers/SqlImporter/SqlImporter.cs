using FwaEu.Fwamework.Data.Database.Nhibernate;
using FwaEu.Modules.Importers.SqlImporter;
using FwaEu.Fwamework.Imports;
using FwaEu.Fwamework.ProcessResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FwaEu.TestTools.InMemoryDatabase.Sqlite;

namespace FwaEu.Modules.Tests.Importers.SqlImporter
{
	[TestClass]
	public class SqlImporterTests
	{
		[TestMethod]
		public async Task ImportSqlFileAsyncTest()
		{

			var createTableQuery = $"CREATE TABLE animals ( id INT, name VARCHAR(100) )";

			var path = Path.GetFullPath("Importers/SqlImporter/Test.sql");
			var file = new FileInfoImportFile(new FileInfo(path));
			var services = new ServiceCollection();
			var options = new InMemoryDatabaseCreationOptions(repositories => { });

			using (var dataScope = InMemoryDataScopeFactory.CreateStatefulInMemoryDataScope(services, options))
			{
				var serviceProvider = services.BuildServiceProvider();
				var sessionAdapterFactory = serviceProvider.GetRequiredService<INhibernateSessionAdapterFactory>();
				var session = (INhibernateStatefulSessionAdapter)dataScope.Session;

				await session.NhibernateSession.CreateSQLQuery(createTableQuery).ExecuteUpdateAsync();

				var countBeforeImport = Convert.ToInt32(await session.NhibernateSession.CreateSQLQuery("Select count (*) from animals").UniqueResultAsync());
			
				Assert.AreEqual(0, countBeforeImport);

				await new FwaEu.Modules.Importers.SqlImporter.SqlImporter(sessionAdapterFactory)
					.ImportAsync(new ImportContext(new ProcessResult(), new IImportFile[] { file }), new IImportFile[] { file });

				var countAfterImport = Convert.ToInt32(await session.NhibernateSession.CreateSQLQuery("Select count (*) from animals").UniqueResultAsync());

				Assert.AreEqual(1, countAfterImport);
			}
		}
	}
}

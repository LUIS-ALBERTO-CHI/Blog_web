using FwaEu.Fwamework.Data.Database.Nhibernate;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Modules.Reports;
using FwaEu.TestTools.InMemoryDatabase.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.Reports
{
	[TestClass]
	public class SqlReportDataProviderTests
	{
		private static DictionaryParametersProviderStub CreateStub(string key, object value)
		{
			return new DictionaryParametersProviderStub(new Dictionary<string, object>()
			{
				{ key, value }
			});
		}

		[TestMethod]
		public async Task LoadDataAsync()
		{

			var createTableQuery = $"CREATE TABLE cats ( id INT, name VARCHAR(100) )";
			var insertQuery = $"INSERT INTO cats (id, name) VALUES (1, 'Felix'), (42, 'Garfield')";

			var services = new ServiceCollection();
			services.AddTransient<IParametersProvider>(sp => CreateStub("Key1", 1));
			services.AddTransient<IParametersProvider>(sp => CreateStub("Key2", 42));
			services.AddTransient<IParametersService, DefaultParametersService>();

			var options = new InMemoryDatabaseCreationOptions(repositories => { });

			using (var dataScope = InMemoryDataScopeFactory.CreateStatefulInMemoryDataScope(services, options))
			{
				var serviceProvider = services.BuildServiceProvider();
				var sessionAdapterFactory = serviceProvider.GetRequiredService<ISessionAdapterFactory>();
				var parametersService = serviceProvider.GetRequiredService<IParametersService>();
				var parameters = await parametersService.LoadParametersAsync();

				var session = (INhibernateStatefulSessionAdapter)dataScope.Session;

				await session.NhibernateSession.CreateSQLQuery(createTableQuery).ExecuteUpdateAsync();
				await session.NhibernateSession.CreateSQLQuery(insertQuery).ExecuteUpdateAsync();

				var provider = new SqlReportDataProvider(sessionAdapterFactory);

				var queryResult = await provider.LoadDataAsync("select * from cats where id = :Key2",
					parameters, CancellationToken.None);

				var rows = new Dictionary<string, object>()
				{
					{ "Id", 42 },
					{ "Name", "Garfield"}
				};

				CollectionAssert.AreEqual(rows, queryResult.Rows[0]);
			}
		}
	}
}



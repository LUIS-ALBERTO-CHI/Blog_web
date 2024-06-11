using FwaEu.Fwamework.Data.Database.Nhibernate;
using FwaEu.Fwamework.Data.Database.Nhibernate.Interceptors;
using FwaEu.TestTools.InMemoryDatabase.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Tests.Data.Nhibernate
{
	[TestClass]
	public class CompositeDispatchInterceptorTests
	{
		[TestMethod]
		public async Task OnPrepareStatement()
		{
			var queryCount = 0;
			var countQueriesInterceptor = new CountQueriesDispatchInterceptorMock(sql => queryCount++);

			var services = new ServiceCollection();
			services.AddSingleton<IDispatchInterceptor>(countQueriesInterceptor);

			var options = new InMemoryDatabaseCreationOptions(repositories => { });
			using (var dataScope = InMemoryDataScopeFactory.CreateStatefulInMemoryDataScope(services, options))
			{
				var session = (INhibernateStatefulSessionAdapter)dataScope.Session;
				const int QueryToExecuteCount = 3;

				for (var i = 1; i <= QueryToExecuteCount; i++)
				{
					await session.NhibernateSession
						.CreateSQLQuery($"SELECT {i}")
						.ExecuteUpdateAsync();
				}

				Assert.AreEqual(QueryToExecuteCount, queryCount);
			}
		}
	}
}

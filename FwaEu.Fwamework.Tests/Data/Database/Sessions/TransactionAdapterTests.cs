using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.TestTools.InMemoryDatabase.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Tests.Data.Database.Sessions
{
	[TestClass]
	public class TransactionAdapterTests
	{
		[TestMethod]
		public async Task CommitAsync()
		{
			await CheckTransactionResultAsync(
				transaction => transaction.CommitAsync(),
				count => Assert.AreEqual(1, count, "Number of cats in database must be equal to one.")
				);
		}

		[TestMethod]
		public async Task RollbackAsync()
		{
			await CheckTransactionResultAsync(
				transaction => transaction.RollbackAsync(),
				count => Assert.AreEqual(0, count, "Number of cats in database must be equal to zero.")
				);
		}

		private async Task CheckTransactionResultAsync(Func<ITransaction, Task> transactionAction, Action<int> assertAction)
		{
			var options = new InMemoryDatabaseCreationOptions(repositories =>
							repositories.Add<CatEntityRepository>(),
							typeof(CatEntityClassmap));

			var services = new ServiceCollection();

			using (var dataScope = InMemoryDataScopeFactory.CreateStatelessInMemoryDataScope(services, options))
			{
				using (var transaction = dataScope.Session.BeginTransaction())
				{
					var cat = new CatEntity { Name = "One", Birthdate = DateTime.Today, Color = "White" };
					await dataScope.Session.SaveOrUpdateAsync(cat);
					await transactionAction(transaction);
				}
				var count = dataScope.Session.Query<CatEntity>().Count();
				assertAction(count);
			}
		}
	}
}

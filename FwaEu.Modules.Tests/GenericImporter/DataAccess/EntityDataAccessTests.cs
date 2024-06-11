using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Users;
using FwaEu.Modules.GenericImporter;
using FwaEu.Modules.GenericImporter.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FwaEu.TemplateCore.Users;
using FwaEu.TestTools.InMemoryDatabase.Sqlite;

namespace FwaEu.Modules.Tests.GenericImporter.DataAccess
{
	[TestClass]
	public class EntityDataAccessTests
	{
		private static StatelessInMemoryDataScope DataScope;
		private static DataAccessUserEntityRepository Repository;
		private static DataAccessUserEntity[] Users;

		[ClassInitialize]
		public async static Task InitializeAsync(TestContext _)
		{
			var options = new InMemoryDatabaseCreationOptions(repositories =>
				repositories.Add<DataAccessUserEntityRepository>(),
				typeof(DataAccessUserEntityClassMap));

			DataScope = InMemoryDataScopeFactory.CreateStatelessInMemoryDataScope(new ServiceCollection(), options);
			Repository = DataScope.CreateRepository<DataAccessUserEntityRepository>();

			await Repository.QueryNoPerimeter().DeleteAsync(CancellationToken.None);

			Users = new[]
			{
				new DataAccessUserEntity()
				{
					FirstName = "Dimitri",
					LastName = "Test",
					Email = "dimitri@fwa.eu",
				},

				new DataAccessUserEntity()
				{
					FirstName = "Dimitri",
					LastName = "Test2",
					Email = "dimitri2@fwa.eu",
				}
			};

			foreach (var user in Users)
			{
				await Repository.SaveOrUpdateAsync(user);
			}
		}

		[ClassCleanup]
		public static void CleanUp()
		{
			Users = null;
			Repository = null;

			DataScope.Dispose();
			DataScope = null;
		}

		[TestMethod]
		public async Task FindAsync_MustNotFind()
		{
			var dataAccess = new EntityDataAccess<DataAccessUserEntity>(Repository);

			var keys = new[]
			{
				new PropertyValueSet("Roberto", nameof(ApplicationUserEntity.FirstName), typeof(string))
			};

			var user = await dataAccess.FindAsync(keys);
			Assert.IsNull(user);
		}

		[TestMethod]
		public async Task FindAsync_MustFind()
		{
			var dataAccess = new EntityDataAccess<DataAccessUserEntity>(Repository);

			var keys = new[]
			{
				new PropertyValueSet("Dimitri", nameof(ApplicationUserEntity.FirstName), typeof(string)),
				new PropertyValueSet("dimitri@fwa.eu", nameof(ApplicationUserEntity.Email), typeof(string)),
			};

			var user = await dataAccess.FindAsync(keys);
			Assert.IsNotNull(user);
		}

		[TestMethod]
		public async Task FindAsync_MustFail()
		{
			var dataAccess = new EntityDataAccess<DataAccessUserEntity>(Repository);

			var keys = new[]
			{
				new PropertyValueSet("Dimitri", nameof(ApplicationUserEntity.FirstName), typeof(string))
			};

			await Assert.ThrowsExceptionAsync<InvalidOperationException>(
				async () => await dataAccess.FindAsync(keys));
		}

		[TestMethod]
		public async Task GetAllAsync()
		{
			var dataAccess = new EntityDataAccess<DataAccessUserEntity>(Repository);
			var usersFromDataAccess = await dataAccess.GetAllAsync();

			Assert.AreEqual(Users.Length, usersFromDataAccess.Length);

			var matches = usersFromDataAccess.Join(Users,
					uda => new { uda.FirstName, uda.LastName, uda.Email },
					u => new { u.FirstName, u.LastName, u.Email },
					(uda, u) => new { uda.Email })
				.ToArray();

			Assert.AreEqual(Users.Length, matches.Length);
		}

		[TestMethod]
		public async Task SaveOrUpdateAsync()
		{
			var dataAccess = new EntityDataAccess<DataAccessUserEntity>(Repository);

			using (var transaction = DataScope.Session.BeginTransaction())
			{
				var firstUser = (await dataAccess.GetAllAsync()).First();
				firstUser.Email = "coucou@fwa.eu";

				await dataAccess.SaveOrUpdateAsync(firstUser);

				var updatedUser = (await dataAccess.GetAllAsync()).First(u => u.Id == firstUser.Id);
				Assert.AreEqual(firstUser.Email, updatedUser.Email);

				await transaction.RollbackAsync();
			}
		}
	}
}

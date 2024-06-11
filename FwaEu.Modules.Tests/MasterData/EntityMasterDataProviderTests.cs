using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Modules.MasterData;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FwaEu.Fwamework.Globalization;
using FwaEu.TestTools.InMemoryDatabase.Sqlite;
using FwaEu.Modules.Tests.Globalization;

namespace FwaEu.Modules.Tests.MasterData
{
	[TestClass]
	public class EntityMasterDataProviderTests
	{
		[TestMethod]
		public async Task GetChangesInfoAsync()
		{
			var options = new InMemoryDatabaseCreationOptions(repositories =>
				repositories.Add<MasterDataEntityRepository>(),
				typeof(MasterDataEntityClassMap));

			var services = new ServiceCollection();

			var now = DateTime.Now;

			var data = new[]
			{
				new MasterDataEntity() { Name = "Bear", UpdatedOn = now.Date.AddDays(-30.0) },
				new MasterDataEntity() { Name = "Teddy", UpdatedOn = now.Date.AddDays(-20.0) },
				new MasterDataEntity() { Name = "Ours", UpdatedOn = now.Date.AddDays(-10.0) },
			};

			using (var dataScope = InMemoryDataScopeFactory.CreateStatefulInMemoryDataScope(services, options))
			{
				var repository = dataScope.CreateRepository<MasterDataEntityRepository>();
				await repository.QueryNoPerimeter().DeleteAsync(CancellationToken.None);
				foreach (var entry in data)
				{
					await repository.SaveOrUpdateAsync(entry);
				}

				await dataScope.Session.FlushAsync();

				var provider = new MasterDataEntityMasterDataProvider(
					dataScope.ServiceProvider.GetRequiredService<MainSessionContext>(),
					new FrenchEnglishCulturesServiceStub());

				var changesInfo = await provider.GetChangesInfoAsync(new MasterDataProviderGetChangesParameters());
				Assert.AreEqual(data.Length, changesInfo.Count);

				var maximumUpdatedOn = data.Max(e => e.UpdatedOn);
				Assert.AreEqual(maximumUpdatedOn, changesInfo.MaximumUpdatedOn);
			}
		}

		[TestMethod]
		public async Task GetModelsAsync()
		{
			var options = new InMemoryDatabaseCreationOptions(repositories =>
				repositories.Add<MasterDataEntityRepository>(),
				typeof(MasterDataEntityClassMap));

			var services = new ServiceCollection();

			var now = DateTime.Now;

			var data = new[]
			{
				new MasterDataEntity() { Name = "Bear", UpdatedOn = now.Date.AddDays(-30.0) },
				new MasterDataEntity() { Name = "Teddy", UpdatedOn = now.Date.AddDays(-20.0) },
				new MasterDataEntity() { Name = "Ours", UpdatedOn = now.Date.AddDays(-10.0) },
			};

			using (var dataScope = InMemoryDataScopeFactory.CreateStatefulInMemoryDataScope(services, options))
			{
				var repository = dataScope.CreateRepository<MasterDataEntityRepository>();
				await repository.QueryNoPerimeter().DeleteAsync(CancellationToken.None);

				foreach (var entry in data)
				{
					await repository.SaveOrUpdateAsync(entry);
				}

				await dataScope.Session.FlushAsync();

				var provider = new MasterDataEntityMasterDataProvider(
					dataScope.ServiceProvider.GetRequiredService<MainSessionContext>(),
					new FrenchEnglishCulturesServiceStub());

				var dataFromProvider = await provider.GetModelsAsync(
					new MasterDataProviderGetModelsParameters(null, null, null, new CultureInfo("fr-FR")));

				var matches = dataFromProvider.Join(data, dfp => dfp.Id, d => d.Id,
					(dfp, d) => new { d.Id })
					.ToArray();

				Assert.AreEqual(matches.Length, data.Length);
			}
		}

		private static MasterDataOrderByEntity[] GetDataForOrderByTests()
		{
			return new[]
{
				new MasterDataOrderByEntity() { FirstName = "Alice", LastName = "A" },
				new MasterDataOrderByEntity() { FirstName = "Albert", LastName = "B" },
				new MasterDataOrderByEntity() { FirstName = "Bob", LastName = "C" },
				new MasterDataOrderByEntity() { FirstName = "Bob", LastName = "D" },
				new MasterDataOrderByEntity() { FirstName = "Carlos", LastName = "D" },
			};
		}

		private static async Task<MasterDataOrderByEntityMasterDataProvider> FillRepositoryAndGetProviderAsync(StatefulInMemoryDataScope dataScope,
			ICulturesService cultureService)
		{
			var repository = dataScope.CreateRepository<MasterDataOrderByEntityRepository>();
			await repository.QueryNoPerimeter().DeleteAsync(CancellationToken.None);

			foreach (var entry in GetDataForOrderByTests())
			{
				await repository.SaveOrUpdateAsync(entry);
			}

			await dataScope.Session.FlushAsync();

			return new MasterDataOrderByEntityMasterDataProvider(
					dataScope.ServiceProvider.GetRequiredService<MainSessionContext>(),
					cultureService);
		}

		[TestMethod]
		public async Task GetModels_OrderBy_Descending()
		{
			var options = new InMemoryDatabaseCreationOptions(repositories =>
				repositories.Add<MasterDataOrderByEntityRepository>(),
				typeof(MasterDataOrderByEntityClassMap));

			var services = new ServiceCollection();

			using (var dataScope = InMemoryDataScopeFactory.CreateStatefulInMemoryDataScope(services, options))
			{
				var cultureService = new FrenchEnglishCulturesServiceStub();
				var provider = await FillRepositoryAndGetProviderAsync(dataScope, cultureService);

				var sortingParameters = new MasterDataProviderGetModelsParameters(null, null,
					new OrderByParameter[] { new OrderByParameter(nameof(MasterDataOrderByEntity.FirstName), false) }, cultureService.FrenchCulture);

				var dataFromProvider = await provider.GetModelsAsync(sortingParameters);
				var sortedArrayName = dataFromProvider.Select(e => e.FirstName).ToArray();

				var sortedArrayData = GetDataForOrderByTests().OrderByDescending(x => x.FirstName).Select(x => x.FirstName).ToArray();

				CollectionAssert.AreEqual(sortedArrayName, sortedArrayData);
			}
		}

		[TestMethod]
		public async Task GetModels_OrderBy_Ascending()
		{
			var options = new InMemoryDatabaseCreationOptions(repositories =>
				repositories.Add<MasterDataOrderByEntityRepository>(),
				typeof(MasterDataOrderByEntityClassMap));

			var services = new ServiceCollection();

			using (var dataScope = InMemoryDataScopeFactory.CreateStatefulInMemoryDataScope(services, options))
			{
				var cultureService = new FrenchEnglishCulturesServiceStub();
				var provider = await FillRepositoryAndGetProviderAsync(dataScope, cultureService);

				var sortingParameters = new MasterDataProviderGetModelsParameters(null, null,
				new OrderByParameter[] { new OrderByParameter(nameof(MasterDataOrderByEntity.FirstName), true) }, cultureService.FrenchCulture);

				var dataFromProvider = await provider.GetModelsAsync(sortingParameters);
				var sortedArrayName = dataFromProvider.Select(e => e.FirstName).ToArray();

				var sortedArrayData = GetDataForOrderByTests().OrderBy(x => x.FirstName).Select(x => x.FirstName).ToArray();

				CollectionAssert.AreEqual(sortedArrayName, sortedArrayData);
			}
		}

		[TestMethod]
		public async Task GetModels_OrderBy_MultipleProperties()
		{
			var options = new InMemoryDatabaseCreationOptions(repositories =>
				repositories.Add<MasterDataOrderByEntityRepository>(),
				typeof(MasterDataOrderByEntityClassMap));

			var services = new ServiceCollection();

			using (var dataScope = InMemoryDataScopeFactory.CreateStatefulInMemoryDataScope(services, options))
			{
				var cultureService = new FrenchEnglishCulturesServiceStub();
				var provider = await FillRepositoryAndGetProviderAsync(dataScope, cultureService);

				var sortingParameters = new MasterDataProviderGetModelsParameters(null, null,
				new OrderByParameter[]
				{
					new OrderByParameter(nameof(MasterDataOrderByEntity.FirstName), true),
					new OrderByParameter(nameof(MasterDataOrderByEntity.LastName), false),
				}, cultureService.FrenchCulture);

				var dataFromProvider = await provider.GetModelsAsync(sortingParameters);
				var sortedArrayName = dataFromProvider.Select(e => new { e.FirstName, e.LastName }).ToArray();

				var sortedArrayData = GetDataForOrderByTests().OrderBy(x => x.FirstName)
					.ThenByDescending(x => x.LastName).Select(e => new { e.FirstName, e.LastName }).ToArray();

				CollectionAssert.AreEqual(sortedArrayName, sortedArrayData);
			}
		}

		[TestMethod]
		public async Task GetModels_Filter()
		{
			var options = new InMemoryDatabaseCreationOptions(repositories =>
				repositories.Add<MasterDataEntityRepository>(),
				typeof(MasterDataEntityClassMap));
			var services = new ServiceCollection();
			var now = DateTime.Now;
			var data = new[]
			{
				new MasterDataEntity() { Name = "Pikachu", UpdatedOn = now.Date.AddDays(-30.0) },
				new MasterDataEntity() { Name = "Picadilly", UpdatedOn = now.Date.AddDays(-20.0) },
				new MasterDataEntity() { Name = "Pikapika", UpdatedOn = now.Date.AddDays(-10.0) },
				new MasterDataEntity() { Name = "Kurapika", UpdatedOn = now.Date.AddDays(-30.0) },
				new MasterDataEntity() { Name = "Paprika", UpdatedOn = now.Date.AddDays(-30.0) },
			};
			using (var dataScope = InMemoryDataScopeFactory.CreateStatefulInMemoryDataScope(services, options))
			{
				var repository = dataScope.CreateRepository<MasterDataEntityRepository>();
				await repository.QueryNoPerimeter().DeleteAsync(CancellationToken.None);
				foreach (var entry in data)
				{
					await repository.SaveOrUpdateAsync(entry);
				}
				await dataScope.Session.FlushAsync();

				var cultureService = new FrenchEnglishCulturesServiceStub();

				var provider = new MasterDataEntityMasterDataProvider(
					dataScope.ServiceProvider.GetRequiredService<MainSessionContext>(),
					cultureService);

				var sortingParameters = new MasterDataProviderGetModelsParameters(null, "Pika", null, cultureService.FrenchCulture);
				var dataFromProvider = await provider.GetModelsAsync(sortingParameters);
				var sortedArrayName = dataFromProvider.Select(x => x.Name).ToArray();
				var sortedArrayData = data.Where(x => x.Name.Contains("Pika", StringComparison.InvariantCultureIgnoreCase))
					.Select(x => x.Name).ToArray();
				CollectionAssert.AreEquivalent(sortedArrayName, sortedArrayData);
			}
		}

		[TestMethod]
		public async Task GetModels_Pagination()
		{
			var options = new InMemoryDatabaseCreationOptions(repositories =>
				repositories.Add<MasterDataEntityRepository>(),
				typeof(MasterDataEntityClassMap));

			var services = new ServiceCollection();

			var now = DateTime.Now;

			var data = new[]
			{
				new MasterDataEntity() { Name = "Alice", UpdatedOn = now.Date.AddDays(-30.0) },
				new MasterDataEntity() { Name = "Bob", UpdatedOn = now.Date.AddDays(-20.0) },
				new MasterDataEntity() { Name = "Carlos", UpdatedOn = now.Date.AddDays(-10.0) },
				new MasterDataEntity() { Name = "Denise", UpdatedOn = now.Date.AddDays(-30.0) },
				new MasterDataEntity() { Name = "Eric", UpdatedOn = now.Date.AddDays(-30.0) },
				new MasterDataEntity() { Name = "Fred", UpdatedOn = now.Date.AddDays(-30.0) },
				new MasterDataEntity() { Name = "Gerard", UpdatedOn = now.Date.AddDays(-30.0) },
				new MasterDataEntity() { Name = "Henri", UpdatedOn = now.Date.AddDays(-30.0) },
				new MasterDataEntity() { Name = "Ingrid", UpdatedOn = now.Date.AddDays(-30.0) },
			};

			using (var dataScope = InMemoryDataScopeFactory.CreateStatefulInMemoryDataScope(services, options))
			{
				var repository = dataScope.CreateRepository<MasterDataEntityRepository>();
				await repository.QueryNoPerimeter().DeleteAsync(CancellationToken.None);
				foreach (var entry in data)
				{
					await repository.SaveOrUpdateAsync(entry);
				}
				await dataScope.Session.FlushAsync();

				var cultureService = new FrenchEnglishCulturesServiceStub();

				var provider = new MasterDataEntityMasterDataProvider(
					dataScope.ServiceProvider.GetRequiredService<MainSessionContext>(),
					cultureService);

				const int toSkip = 2;
				const int toTake = 5;

				var sortingParameters = new MasterDataProviderGetModelsParameters(new MasterDataPaginationParameters(toSkip, toTake), null, null, cultureService.FrenchCulture);
				var dataFromProvider = await provider.GetModelsAsync(sortingParameters);
				var sortedArrayName = dataFromProvider.Select(x => x.Name).ToArray();
				var sortedArrayData = data.Select(x => x.Name).Skip(toSkip).Take(toTake).ToArray();

				CollectionAssert.AreEqual(sortedArrayName, sortedArrayData);
			}
		}
	}
}
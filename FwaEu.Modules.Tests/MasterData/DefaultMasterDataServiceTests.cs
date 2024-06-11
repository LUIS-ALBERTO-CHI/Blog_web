using FwaEu.Modules.MasterData;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.MasterData
{
	[TestClass]
	public class DefaultMasterDataServiceTests
	{
		[TestMethod]
		[DataRow(1, DisplayName = "Must return result only for A")]
		[DataRow(2, DisplayName = "Must return result for A and B")]
		public async Task GetChangesInfosAsync_CountOfResults(int numberOfParametersToCreate)
		{
			var services = new ServiceCollection();
			var now = DateTime.Now;

			using (var serviceProvider = services.BuildServiceProvider())
			{
				var masterDataservice = new DefaultMasterDataService(
					new MasterDataFactoryCache(new[]
					{
						new MasterDataProviderFactoryStub("Key1",
							new MasterDataProviderStub(now, 2, new []{ "Key1_1", "Key1_2" })),
						new MasterDataProviderFactoryStub("Key2",
							new MasterDataProviderStub(now.AddDays(-10), 3, new []{ "Key2_1", "Key2_2", "Key2_3"}))
					}),
					serviceProvider);

				var keys = Enumerable.Range(1, numberOfParametersToCreate)
					.Select(i => $"Key{i}")
					.ToArray();

				var parameters = keys
					.Select(key => new RelatedParameters<MasterDataProviderGetChangesParameters>(key,
						new MasterDataProviderGetChangesParameters()))
					.ToArray();

				var data = await masterDataservice.GetChangesInfosAsync(parameters);
				var matches = data.Join(keys, d => d.Key, k => k, (d, k) => new { Key = k }).ToArray();

				Assert.AreEqual(data.Length, matches.Length);
				Assert.AreEqual(parameters.Length, matches.Length);
			}
		}

		[TestMethod]
		[DataRow(1, DisplayName = "Must return result only for A")]
		[DataRow(2, DisplayName = "Must return result for A and B")]
		public async Task GetModelsAsync_CountOfResults(int numberOfParametersToCreate)
		{
			var services = new ServiceCollection();
			var now = DateTime.Now;

			using (var serviceProvider = services.BuildServiceProvider())
			{
				var masterDataservice = new DefaultMasterDataService(
					new MasterDataFactoryCache(new[]
					{
						new MasterDataProviderFactoryStub("Key1",
							new MasterDataProviderStub(now, 2, new []{ "Key1_1", "Key1_2" })),
						new MasterDataProviderFactoryStub("Key2",
							new MasterDataProviderStub(now.AddDays(-10), 3, new []{ "Key2_1", "Key2_2", "Key2_3"}))
					}),
					serviceProvider);

				var keys = Enumerable.Range(1, numberOfParametersToCreate)
					.Select(i => $"Key{i}")
					.ToArray();

				var parameters = keys
					.Select(key => new RelatedParameters<MasterDataProviderGetModelsParameters>(key,
						new MasterDataProviderGetModelsParameters(null, null, null, new CultureInfo("fr-FR"))))
					.ToArray();

				var data = await masterDataservice.GetModelsAsync(parameters);
				var matches = data.Join(keys, d => d.Key, k => k, (d, k) => new { Key = k }).ToArray();

				Assert.AreEqual(data.Length, matches.Length);
				Assert.AreEqual(matches.Length, parameters.Length);
			}
		}
	}
}

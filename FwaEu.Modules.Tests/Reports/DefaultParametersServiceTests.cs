using FwaEu.Modules.Reports;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.Reports
{
	[TestClass]
	public class DefaultParametersServiceTests
	{
		private static DictionaryParametersProviderStub CreateStub(string key, object value)
		{
			return new DictionaryParametersProviderStub(new Dictionary<string, object>()
			{
				{ key, value }
			});
		}

		[TestMethod]
		public async Task LoadParametersAsync_MergeDictionaries()
		{
			var serviceCollection = new ServiceCollection();
			serviceCollection.AddTransient<IParametersProvider>(sp => CreateStub("Key1", "Value1"));
			serviceCollection.AddTransient<IParametersProvider>(sp => CreateStub("Key2", "Value2"));

			using (var serviceProvider = serviceCollection.BuildServiceProvider())
			{
				var service = new DefaultParametersService(serviceProvider);
				var result = await service.LoadParametersAsync();

				Assert.IsTrue(result.Parameters.ContainsKey("Key1"));
				Assert.AreEqual("Value1", result.Parameters["Key1"]);

				Assert.IsTrue(result.Parameters.ContainsKey("Key2"));
				Assert.AreEqual("Value2", result.Parameters["Key2"]);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public async Task LoadParametersAsync_KeyAlreadyExists()
		{
			var serviceCollection = new ServiceCollection();
			serviceCollection.AddTransient<IParametersProvider>(sp => CreateStub("Key", "Value A"));
			serviceCollection.AddTransient<IParametersProvider>(sp => CreateStub("Key", "Value B"));

			using (var serviceProvider = serviceCollection.BuildServiceProvider())
			{
				var service = new DefaultParametersService(serviceProvider);
				var result = await service.LoadParametersAsync(); // Must raise an exception when creating the resulting dictionary (merged)
			}
		}
	}
}

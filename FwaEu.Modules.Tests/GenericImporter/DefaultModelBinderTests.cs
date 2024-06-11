using FwaEu.Fwamework;
using FwaEu.Fwamework.ValueConverters;
using FwaEu.Modules.GenericImporter;
using FwaEu.Modules.GenericImporter.DataAccess;
using FwaEu.Modules.Tests.GenericImporterCommon;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.GenericImporter
{
	[TestClass]
	public class DefaultModelBinderTests
	{
		[TestMethod]
		[DataRow("Romain", 12)]
		[DataRow("Romdeux", 17)]
		public async Task BindToModelAsync(string name, int age)
		{
			var services = new ServiceCollection();
			var serviceProvider = services.BuildServiceProvider();

			var convertService = new DefaultValueConvertService(serviceProvider);

			var binder = new DefaultModelBinder<UserModel>(convertService, new DefaultPropertyBinder());
			var model = new UserModel();

			Assert.IsNull(model.Name);
			Assert.AreEqual(0, model.Age);

			var metadataProperties = new[]
			{
				new ModelPropertyDescriptor(nameof(UserModel.Name), IsKeyValue.True, false, null, nameof(UserModel.Name)),
				new ModelPropertyDescriptor(nameof(UserModel.Age), IsKeyValue.False, false, null, nameof(UserModel.Age)),
			};

			var dataRow = new DataRow(metadataProperties, new Dictionary<string, object>()
			{
				{  metadataProperties[0].Name, name },
				{  metadataProperties[1].Name, age }
			});

			await binder.BindToModelAsync(model, dataRow,
				new ServiceStore(ServiceStoreInnerServicesLifetime.Scoped));

			Assert.AreEqual(name, model.Name);
			Assert.AreEqual(age, model.Age);
		}


		[TestMethod]
		public async Task BindToModelAsync_SearchOn()
		{
			var childToSearch = new SearchOnRelatedTestModel() { Name = "Child", Age = 10 };

			var childrenTestSet = new[]
			{
				new SearchOnRelatedTestModel(){ Name = "Youpi", Age = 10 },
				new SearchOnRelatedTestModel(){ Name = "Youpa", Age = 11},
				new SearchOnRelatedTestModel(){ Name = childToSearch.Name, Age = 13 },
				childToSearch
			};

			var services = new ServiceCollection();

			services.AddSingleton<IDataAccessFactory<SearchOnRelatedTestModel>>(
				new SearchOnRelatedTestModelDataAccessFakeFactory(childrenTestSet));

			var serviceProvider = services.BuildServiceProvider();

			var convertService = new DefaultValueConvertService(serviceProvider);
			var binder = new DefaultModelBinder<SearchOnTestModel>(convertService, new DefaultPropertyBinder());

			var testDataModel = new SearchOnTestModel()
			{
				Name = "Roberto",
				RelatedModel = new SearchOnRelatedTestModel()
				{
					Name = childToSearch.Name,
					Age = childToSearch.Age,
				},
			};

			var searchOnDataReaderStub = new SearchOnDataReaderStub(new[] { testDataModel });
			var dataRow = searchOnDataReaderStub.GetRows().First();
			var newModel = new SearchOnTestModel();

			using (var serviceStore = new ServiceStore(ServiceStoreInnerServicesLifetime.Scoped))
			{
				var dataAccessProvider = new DataAccessProvider(serviceStore, serviceProvider);
				serviceStore.Add(dataAccessProvider);

				await binder.BindToModelAsync(newModel, dataRow, serviceStore);
			}
			Assert.AreEqual(childToSearch, newModel.RelatedModel);
		}

		[TestMethod]
		[DataRow("Romain", 12)]
		[ExpectedException(typeof(KeyNotFoundException))]
		public async Task BindToModelAsync_MustFail(string name, int age)
		{
			var services = new ServiceCollection();
			var serviceProvider = services.BuildServiceProvider();
			var convertService = new DefaultValueConvertService(serviceProvider);
			var binder = new DefaultModelBinder<UserModel>(convertService, new DefaultPropertyBinder());
			var model = new UserModel();

			var metadataProperties = new[]
			{
				new ModelPropertyDescriptor("LastName", IsKeyValue.True, false, null, "Last Name"),
				new ModelPropertyDescriptor("FirstName", IsKeyValue.True, false, null, "First Name"),
			};

			var dataRow = new DataRow(metadataProperties, new Dictionary<string, object>()
			{
				{  metadataProperties[0].Name, name },
				{  metadataProperties[1].Name, age }
			});

			await binder.BindToModelAsync(model, dataRow,
				new ServiceStore(ServiceStoreInnerServicesLifetime.Scoped));
		}

		[TestMethod]
		[DataRow("Romain", 12)]
		[ExpectedException(typeof(NoMatchingValueException))]
		public async Task BindToModelAsync_SearchOn_MustFail(string name, int age)
		{
			var childToSearch = new SearchOnRelatedTestModel() { Name = "Child", Age = 10 };

			var childrenTestSet = new[]
			{
				new SearchOnRelatedTestModel(){ Name = "Youpi", Age = 10 },
				new SearchOnRelatedTestModel(){ Name = "Youpa", Age = 11},
				new SearchOnRelatedTestModel(){ Name = childToSearch.Name, Age = 13 },
			};

			var services = new ServiceCollection();
			services.AddSingleton<IDataAccessFactory<SearchOnRelatedTestModel>>(
				new SearchOnRelatedTestModelDataAccessFakeFactory(childrenTestSet));

			var serviceProvider = services.BuildServiceProvider();
			var convertService = new DefaultValueConvertService(serviceProvider);
			var binder = new DefaultModelBinder<SearchOnTestModel>(convertService, new DefaultPropertyBinder());

			var testDataModel = new SearchOnTestModel()
			{
				Name = "Roberto",
				RelatedModel = new SearchOnRelatedTestModel()
				{
					Name = childToSearch.Name,
					Age = childToSearch.Age,
				},
			};

			var searchOnDataReaderStub = new SearchOnDataReaderStub(new[] { testDataModel });
			var dataRow = searchOnDataReaderStub.GetRows().First();
			var newModel = new SearchOnTestModel();

			using (var serviceStore = new ServiceStore(ServiceStoreInnerServicesLifetime.Scoped))
			{
				var dataAccessProvider = new DataAccessProvider(serviceStore, serviceProvider);
				serviceStore.Add(dataAccessProvider);

				await binder.BindToModelAsync(newModel, dataRow, serviceStore);
			}
		}
	}
}

using FwaEu.Fwamework.Globalization;
using FwaEu.Fwamework.Users;
using FwaEu.Modules.GenericAdmin;
using FwaEu.Modules.GenericAdmin.WebApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FwaEu.Modules.Tests.GenericAdmin
{
	[TestClass]
	public class ControllerTests
	{
		public class ContextLanguageStub : IUserContextLanguage
		{
			public CultureInfo GetCulture()
			{
				return new CultureInfo("en-US");
			}
		}

		private static ServiceProvider BuildServiceProvider()
		{
			var services = new ServiceCollection();

			services.AddFwameworkModuleGenericAdmin();

			services.AddGenericAdminConfiguration<MockGenericAdminModelConfiguration>(MockGenericAdminModelConfiguration.Key);
			services.AddGenericAdminConfiguration<UnaccessibleGenericAdminModelConfigurationFake>(UnaccessibleGenericAdminModelConfigurationFake.Key);
			services.AddGenericAdminConfiguration<ModelAttributeMockGenericAdminModelConfiguration>(ModelAttributeMockGenericAdminModelConfiguration.Key);

			services.AddSingleton<ICurrentUserService, FakeCurrentUserService>();
			services.AddScoped<IUserContextLanguage, ContextLanguageStub>();

			return services.BuildServiceProvider();
		}

		[TestMethod]
		public async Task GetConfiguration_IsAccessible()
		{
			using (var serviceProvider = BuildServiceProvider())
			{
				var genericAdminService = serviceProvider.GetService<IGenericAdminService>();
				var configurations = serviceProvider.GetServices<IGenericAdminConfigurationFactory>();
				var configuration = configurations
					.FirstOrDefault(c => c.Key == "UnaccessibleTest").Create(serviceProvider);
				Assert.IsFalse(await configuration.IsAccessibleAsync());

				var result = await new GenericAdminController().GetConfiguration("UnaccessibleTest", genericAdminService);

				Assert.AreEqual(typeof(ForbidResult), result.Result.GetType(),
					$"This configuration should raise a '{typeof(ForbidResult).FullName}' exception.");
			}
		}

		[TestMethod]
		public async Task GetConfiguration_PropertiesCopy()
		{
			using (var serviceProvider = BuildServiceProvider())
			{
				var genericAdminService = serviceProvider.GetService<IGenericAdminService>();
				var configuration = serviceProvider.GetServices<IGenericAdminConfigurationFactory>()
					.FirstOrDefault(c => c.Key == "Mock").Create(serviceProvider);

				var actionResult = await new GenericAdminController().GetConfiguration("Mock", genericAdminService);
				var controllerModel = (actionResult.Result as OkObjectResult).Value as ConfigurationModel;

				foreach (var property in configuration.GetProperties())
				{
					var modelProperty = controllerModel.Properties.First(p => p.Name == property.Name);

					Assert.AreEqual(property.IsKey, modelProperty.IsKey);
					Assert.AreEqual(property.Name, modelProperty.Name);
					Assert.AreEqual(property.InnerType.Name, modelProperty.Type);
					CollectionAssert.AreEqual(property.ExtendedProperties, modelProperty.ExtendedProperties);
				}
			}
		}

		[TestMethod]
		public async Task GetConfiguration_ModelsDataLoad()
		{
			using (var serviceProvider = BuildServiceProvider())
			{
				var genericAdminService = serviceProvider.GetService<IGenericAdminService>();
				var currentUserService = serviceProvider.GetService<ICurrentUserService>();
				var configuration = serviceProvider.GetServices<IGenericAdminConfigurationFactory>()
					.FirstOrDefault(c => c.Key == "MockByModelAttribute").Create(serviceProvider);

				var authorizedActions = configuration.GetAuthorizedActions();

				var loadDataResult = await configuration.GetModelsAsync(null);

				var actionResult = await new GenericAdminController().GetModels(
					"MockByModelAttribute",
					null,
					genericAdminService,
					serviceProvider.GetServices<IDataSourceModelFactory>(),
					serviceProvider.GetRequiredService<IUserContextLanguage>());
				var controllerModel = (actionResult.Result as OkObjectResult).Value as DataSourceModel;

				Assert.AreEqual(loadDataResult.Value.Items.Count(), controllerModel.Data.Count);
				CollectionAssert.AreEqual(loadDataResult.Value.Items.ToArray(), controllerModel.Data);
			}
		}
	}
}

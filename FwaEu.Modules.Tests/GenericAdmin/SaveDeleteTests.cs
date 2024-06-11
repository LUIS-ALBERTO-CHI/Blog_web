using FwaEu.Fwamework.ValueConverters;
using FwaEu.Modules.GenericAdmin;
using FwaEu.TemplateCore.Initialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.GenericAdmin
{
	[TestClass]
	public class SaveDeleteTests
	{
		private static void SetTestValues(ModelAttributeMockModel model)
		{
			model.Name = "Test";
			model.CityId = 3;
		}

		private static ServiceProvider BuildServiceProvider()
		{
			var services = new ServiceCollection();

			services.AddFwameworkModuleGenericAdmin();

			services.AddSingleton<IValueConvertService, DefaultValueConvertService>();

			return services.BuildServiceProvider();
		}

		[TestMethod]
		public async Task ModelAttribute_Save()
		{
			using (var serviceProvider = BuildServiceProvider())
			{
				var configuration = new ModelAttributeMockGenericAdminModelConfiguration(serviceProvider);

				var initialLoadDataResult = await configuration.GetModelsAsync(null);
				var firstModelId = initialLoadDataResult.Value.Items.First().Id;

				var modelsToUpdate = new ModelAttributeMockModel[]
				{
					new ModelAttributeMockModel() { Id = firstModelId }, // NOTE: Update
					new ModelAttributeMockModel() // NOTE: New
				};

				foreach (var model in modelsToUpdate)
				{
					SetTestValues(model);
				}

				Assert.AreNotEqual(initialLoadDataResult.Value.Items.First().Name, "Test");

				var saveResult = await configuration.SaveAsync(modelsToUpdate);

				var afterUpdateLoadDataResult = await configuration.GetModelsAsync(null);

				Assert.AreEqual(modelsToUpdate.Length, saveResult.Results.Count());

				Assert.AreEqual(initialLoadDataResult.Value.Items.Count() + 1,
					afterUpdateLoadDataResult.Value.Items.Count());

				Assert.AreEqual("Test", afterUpdateLoadDataResult.Value.Items.First().Name);
			}
		}

		[TestMethod]
		public async Task ModelAttribute_Delete()
		{
			using (var serviceProvider = BuildServiceProvider())
			{
				var configuration = (IGenericAdminModelConfiguration)new ModelAttributeMockGenericAdminModelConfiguration(serviceProvider);

				var initialLoadDataResult = await configuration.GetModelsAsync(null);

				var modelIdToDelete = ((ModelAttributeMockModel)initialLoadDataResult.Value.Items.First()).Id;

				await configuration.DeleteAsync(new[]
				{
					new Dictionary<string, object>() { { nameof(ModelAttributeMockModel.Id) , modelIdToDelete } }
				});

				var afterDeleteLoadDataResult = await configuration.GetModelsAsync(null);
				Assert.AreEqual(initialLoadDataResult.Value.Items.Count() - 1,
					afterDeleteLoadDataResult.Value.Items.Count());
			}
		}

		[TestMethod]
		public async Task ModelAttribute_ProhibitCreate()
		{
			using (var serviceProvider = BuildServiceProvider())
			{
				var authorizedActions = new AuthorizedActions() { AllowCreate = false };
				var configuration = new ModelAttributeMockGenericAdminModelConfiguration(serviceProvider, authorizedActions);

				var initialLoadDataResult = await configuration.GetModelsAsync(null);

				var modelsToUpdate = new ModelAttributeMockModel[]
				{
					new ModelAttributeMockModel() // NOTE: New
				};

				var exceptionRaised = false;

				try
				{
					await configuration.SaveAsync(modelsToUpdate);
				}
				catch (AuthorizationException)
				{
					exceptionRaised = true;
				}

				Assert.IsTrue(exceptionRaised,
					$"The action method should raise an action of type '{typeof(AuthorizationException).FullName}'.");
			}
		}

		[TestMethod]
		public async Task ModelAttribute_ProhibitUpdate()
		{
			using (var serviceProvider = BuildServiceProvider())
			{
				var authorizedActions = new AuthorizedActions() { AllowUpdate = false };
				var configuration = new ModelAttributeMockGenericAdminModelConfiguration(serviceProvider, authorizedActions);

				var initialLoadDataResult = await configuration.GetModelsAsync(null);
				var firstModelId = initialLoadDataResult.Value.Items.First().Id;

				var modelsToUpdate = new ModelAttributeMockModel[]
				{
					new ModelAttributeMockModel() { Id = firstModelId }, // NOTE: Update
					new ModelAttributeMockModel() // NOTE: New
				};

				var exceptionRaised = false;

				try
				{
					await configuration.SaveAsync(modelsToUpdate);
				}
				catch (AuthorizationException)
				{
					exceptionRaised = true;
				}

				Assert.IsTrue(exceptionRaised,
					$"The action method should raise an action of type '{typeof(AuthorizationException).FullName}'.");
			}
		}

		[TestMethod]
		public async Task ModelAttribute_ProhibitDelete()
		{
			using (var serviceProvider = BuildServiceProvider())
			{
				var authorizedActions = new AuthorizedActions() { AllowDelete = false };
				var configuration = (IGenericAdminModelConfiguration)new ModelAttributeMockGenericAdminModelConfiguration(serviceProvider, authorizedActions);

				var initialLoadDataResult = await configuration.GetModelsAsync(null);

				var modelIdToDelete = ((ModelAttributeMockModel)initialLoadDataResult.Value.Items.First()).Id;

				var exceptionRaised = false;

				try
				{
					await configuration.DeleteAsync(new[]
					{
						new Dictionary<string, object>() { { nameof(ModelAttributeMockModel.Id) , modelIdToDelete } }
					});
				}
				catch (AuthorizationException)
				{
					exceptionRaised = true;
				}

				Assert.IsTrue(exceptionRaised,
					$"The action method should raise an action of type '{typeof(AuthorizationException).FullName}'.");
			}
		}
	}
}

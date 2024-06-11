using FwaEu.Modules.GenericAdmin;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace FwaEu.Modules.Tests.GenericAdmin
{
	[TestClass]
	public class ExportTests
	{
		private static ServiceProvider BuildServiceProvider()
		{
			var services = new ServiceCollection();

			services.AddFwameworkModuleGenericAdmin();

			services.AddGenericAdminConfiguration<MockGenericAdminModelConfiguration>(MockGenericAdminModelConfiguration.Key);

			return services.BuildServiceProvider();
		}

		[TestMethod]
		public void FindTestExportByType_ShouldFind()
		{
			using (var serviceProvider = BuildServiceProvider())
			{
				var configurations = serviceProvider.GetServices<IGenericAdminConfigurationFactory>();

				Assert.IsTrue(configurations.Any(c => c.Create(serviceProvider) is MockGenericAdminModelConfiguration));
			}
		}

		[TestMethod]
		public void FindTestExportByKey_ShouldFind()
		{
			using (var serviceProvider = BuildServiceProvider())
			{
				var configurations = serviceProvider.GetServices<IGenericAdminConfigurationFactory>();

				Assert.IsNotNull(configurations.FirstOrDefault(c => c.Key == "Mock"));
			}
		}

		[TestMethod]
		public void FindTestExportByKey_ShouldNotFind()
		{
			using (var serviceProvider = BuildServiceProvider())
			{
				var configurations = serviceProvider.GetServices<IGenericAdminConfigurationFactory>();

				Assert.IsNull(configurations.FirstOrDefault(c => c.Key == "Mock" + DateTime.Now.Ticks.ToString()));
			}
		}
	}
}

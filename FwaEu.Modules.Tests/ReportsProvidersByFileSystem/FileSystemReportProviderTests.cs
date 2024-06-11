using FwaEu.Fwamework.Configuration;
using FwaEu.Fwamework.Globalization;
using FwaEu.Fwamework.Users;
using FwaEu.Modules.ReportsProvidersByFileSystem;
using FwaEu.Modules.Tests.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.ReportsProvidersByFileSystem
{

	public class ContextLanguageStub : IUserContextLanguage
	{
		public CultureInfo GetCulture()
		{
			return new CultureInfo("en-US");
		}
	}

	[TestClass]
	public class FileSystemReportProviderTests
	{

		private static IEnumerable<CultureInfo> GetCultures()
		{
			yield return new CultureInfo("en-US");
			yield return new CultureInfo("fr-FR");
		}
		private static ServiceProvider BuildServiceProvider()
		{
			var cultures = GetCultures().ToArray();
			var services = new ServiceCollection();
			services.AddTransient<IHostEnvironment, HostEnvironmentFake>();
			services.AddFwameworkConfigurations();
			services.AddSingleton<ICulturesService>(
				new DefaultCulturesService(cultures.First(), cultures));
			services.AddScoped<IUserContextLanguage, ContextLanguageStub>();

			return services.BuildServiceProvider();
		}

		[TestMethod]
		public async Task FindByInvariantIdAsync_Users()
		{
			using (var serviceProvider = BuildServiceProvider())
			{
				var service = new FileSystemReportProvider(
					serviceProvider.GetRequiredService<IPathFileProvider>(),
					new OptionsReportsProvidersByFileSystemOptionsStub(),
					serviceProvider.GetService<ICulturesService>());

				var model = await service.FindByInvariantIdAsync("Users", new CultureInfo("en-US"));

				Assert.IsNotNull(model);

				Assert.AreEqual("Users list", model.Report.Name,
					"Must be the same value as in the Reports/Users.report.json file.");
			}
		}

		[TestMethod]
		public async Task FindByInvariantIdAsync_NotFound()
		{
			using (var serviceProvider = BuildServiceProvider())
			{
				var service = new FileSystemReportProvider(
					serviceProvider.GetRequiredService<IPathFileProvider>(),
					new OptionsReportsProvidersByFileSystemOptionsStub(),
					serviceProvider.GetRequiredService<ICulturesService>());

				var model = await service.FindByInvariantIdAsync("NotFound", new CultureInfo("en-US"));

				Assert.IsNull(model);
			}
		}

		[TestMethod]
		public async Task GetAllAsync()
		{
			using (var serviceProvider = BuildServiceProvider())
			{
				var service = new FileSystemReportProvider(
					serviceProvider.GetRequiredService<IPathFileProvider>(),
					new OptionsReportsProvidersByFileSystemOptionsStub(),
					serviceProvider.GetRequiredService<ICulturesService>());

				var models = await service.GetAllAsync(new CultureInfo("en-US"));

				var modelsConsumed = models.ToArray();
				var expectedInvariantIds = new[] { "Users", "Dogs" };

				Assert.AreEqual(modelsConsumed.Length, expectedInvariantIds.Length);

				var matches = modelsConsumed.Join(expectedInvariantIds,
					m => m.Report.InvariantId, ii => ii, (m, ii) => ii)
					.ToArray();

				Assert.AreEqual(expectedInvariantIds.Length, matches.Length);
			}
		}
	}
}

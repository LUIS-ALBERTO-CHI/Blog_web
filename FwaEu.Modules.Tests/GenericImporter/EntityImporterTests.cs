using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Nhibernate.Tracking;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Formatting;
using FwaEu.Fwamework.Imports;
using FwaEu.Fwamework.ProcessResults;
using FwaEu.Fwamework.Temporal;
using FwaEu.Fwamework.Users;
using FwaEu.Fwamework.ValueConverters;
using FwaEu.Modules.GenericImporter;
using FwaEu.Modules.Tests.GenericAdmin;
using FwaEu.Modules.Tests.ProcessResults;
using FwaEu.TestTools.InMemoryDatabase.Sqlite;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NHibernate.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.GenericImporter
{
	[TestClass]
	public class EntityImporterTests
	{
		[TestMethod]
		public async Task ImportAsync()
		{
			var options = new InMemoryDatabaseCreationOptions(repositories =>
				repositories.Add<DogEntityRepository>(),
				typeof(DogEntityClassMap));

			var services = new ServiceCollection();
			services.AddFwameworkValueConverters();
			services.AddFwameworkModuleGenericImporter();
			services.AddFwameworkFormatting();
			
			//required services to run importer
			services.AddSingleton<ICurrentDateTime, DateTimeNowCurrentDateTime>();
			services.AddSingleton<ICurrentUserService, FakeCurrentUserService>();
			services.AddSingleton<CreationOrUpdateTrackingEventListener.Disabler>();

			var date = DateTime.Now;

			var data = new[]
			{
				new DogEntity() {  Name= "Rex", Birthdate = date, Price = 100.25m },
				new DogEntity() {  Name= "Milou", Birthdate = date, Price = null },
				new DogEntity() {  Name= "Lassie", Birthdate = date, Price = null },
			};

			using (var dataScope = InMemoryDataScopeFactory.CreateStatefulInMemoryDataScope(services, options))
			{
				var repository = dataScope.CreateRepository<DogEntityRepository>();

				await repository.QueryNoPerimeter().DeleteAsync(CancellationToken.None);

				var existingDogs = new[] { new DogEntity() { Name = "Rex", Birthdate = date.AddDays(-100), Price = 200m } };

				foreach (var existingDog in existingDogs)
				{
					await repository.SaveOrUpdateAsync(existingDog);
				}

				var processResult = new ProcessResult()
				{
					Listener = new ThrowExceptionProcessResultListenerMock(),
				};

				var entityImporter = dataScope.ServiceProvider.GetRequiredService<IModelImporter<DogEntity>>();

				using (var serviceStore = new ServiceStore(ServiceStoreInnerServicesLifetime.Scoped))
				{
					serviceStore.Add(dataScope.Session); //NOTE: To share the session of repository and the one created in EntityDataAccess<>

					var modelImporterContext = new ModelImporterContext(processResult, serviceStore);
					var dataReader = new DogEntityDataReaderStub(data);

					await entityImporter.ImportAsync(dataReader, modelImporterContext);

					var databaseDogs = await repository.QueryNoPerimeter().ToListAsync();

					Assert.AreEqual(data.Length, databaseDogs.Count,
						$"Number of dogs from database must be equals to those created from {nameof(existingDogs)}.");

					var matches = data.Join(databaseDogs,
						d => new { d.Name, d.Birthdate, d.Price },
						dd => new { dd.Name, dd.Birthdate, dd.Price },
						(d, dd) => new { d.Name })
						.ToArray();

					Assert.AreEqual(data.Length, matches.Length,
						$"The dogs from database must be identicals to those " +
						$"in the test set '{nameof(data)}', after '{nameof(entityImporter)}.{nameof(entityImporter.ImportAsync)}'. ");

					Assert.AreEqual(1, processResult.Contexts.Length,
						"Only one context must be created during this test import.");

					var resultContext = processResult.Contexts.First();
					var groups = resultContext.Entries.ToLookup(e => e.Type);

					Assert.AreEqual(data.Length - existingDogs.Length,
						groups[ModelCreatedProcessResultEntry.TypeValue].Count(),
						"Number of dogs that the import process must create.");

					Assert.AreEqual(existingDogs.Length,
						groups[ModelUpdatedProcessResultEntry.TypeValue].Count(),
						"Number of dog that the import process must update.");
				}
			}
		}
	}
}

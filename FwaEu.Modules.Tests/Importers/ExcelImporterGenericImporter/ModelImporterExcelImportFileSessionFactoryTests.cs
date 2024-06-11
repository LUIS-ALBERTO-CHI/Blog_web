using FwaEu.Fwamework;
using FwaEu.Fwamework.Imports;
using FwaEu.Fwamework.ProcessResults;
using FwaEu.Modules.GenericImporter;
using FwaEu.Modules.GenericImporterWorksheet;
using FwaEu.Modules.Importers.ExcelImporter;
using FwaEu.Modules.Importers.ExcelImporterGenericImporter;
using FwaEu.Modules.Tests.GenericImporterCommon;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.Importers.ExcelImporterGenericImporter
{
	[TestClass]
	public class ModelImporterExcelImportFileSessionFactoryTests
	{
		private static (FileInfoImportFile File, ExcelImportSessionFactory ExcelImportSessionFactory) LoadTestFile(ServiceCollection services)
		{
			var serviceProvider = services.BuildServiceProvider();

			var path = Path.GetFullPath("Importers/ExcelImporterGenericImporter/Test.xlsx");
			var file = new FileInfoImportFile(new FileInfo(path));
			var excelImportSessionFactory = new ExcelImportSessionFactory(
					new[] { new ModelImporterExcelImportFileSessionFactory(serviceProvider) });

			return (file, excelImportSessionFactory);
		}

		[TestMethod]
		public async Task FindGenericImporterWorksheets()
		{
			var services = new ServiceCollection();
			var load = LoadTestFile(services);

			await using (var context = new ImportContext(new ProcessResult(), new IImportFile[] { load.File }))
			{
				var importSession = await load.ExcelImportSessionFactory.CreateImportSessionAsync(context, context.Files);
				Assert.IsInstanceOfType(importSession, typeof(ExcelImportFileSession));

				var excelImportSession = (ExcelImportFileSession)importSession;
				Assert.AreEqual(1, excelImportSession.Sessions.Length);

				var session = excelImportSession.Sessions.First();
				Assert.IsInstanceOfType(session, typeof(ModelImporterExcelImportFileSession));
			}
		}

		[TestMethod]
		public async Task FindGenericImporterModelImporter()
		{
			var services = new ServiceCollection();
			var createdImporterSpy = default(UserModelImporter);

			services.AddFwameworkModuleGenericImporter();
			services.AddTransient<IModelImporter<UserModel>, UserModelImporter>(sp =>
		   {
			   Assert.IsNull(createdImporterSpy,
				   $"Only one creation of importer for {nameof(UserModel)} should be done in this test.");

			   return createdImporterSpy = new UserModelImporter();
		   });

			var load = LoadTestFile(services);

			await using (var context = new ImportContext(new ProcessResult(), new IImportFile[] { load.File }))
			{
				var importSession = await load.ExcelImportSessionFactory.CreateImportSessionAsync(context, context.Files);
				await importSession.ImportAsync();

				Assert.IsNotNull(createdImporterSpy,
					$"The UserModelImporter must be created by code inside '{nameof(importSession)}.{nameof(importSession.ImportAsync)}()'.");
			}
		}
	}
}

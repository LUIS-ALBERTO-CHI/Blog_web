using Aspose.Cells;
using FwaEu.Fwamework.Imports;
using FwaEu.Fwamework.ProcessResults;
using FwaEu.Modules.Importers.ExcelImporter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.Importers.ExcelImporter
{
	[TestClass]
	public class ExcelImportSessionFactoryTests
	{
		private static DefaultImportService _service;

		[ClassInitialize]
		public static void Initialize(TestContext context)
		{
			var mock = new TestOpenWorksheetExcelImportFileSessionFactoryMock();
			var factory = new ExcelImportSessionFactory(new[] { mock });

			_service = new DefaultImportService(new[] { factory });
		}

		[TestMethod]
		public async Task OpenWorksheetTest()
		{
			var path = Path.GetFullPath("Importers/ExcelImporter/Test.xlsx");
			var file = new FileInfoImportFile(new FileInfo(path));

			await using (var context = new ImportContext(new ProcessResult(), new IImportFile[] { file }))
			{
				await _service.ImportAllFilesAsync(context);

				var openWorksheetResult = context.ServiceStore.Get<TestOpenExcelImportFileSessionResult>();
				var expectedWorksheetName = TestOpenWorksheetExcelImportFileSessionFactoryMock.OpenExcelTestWorksheetName;

				Assert.IsNotNull(openWorksheetResult, $"Sheet with name '{expectedWorksheetName}' not found.");

				Assert.AreEqual(expectedWorksheetName, openWorksheetResult.ExcelWorksheet.Worksheet.Name);
				Assert.AreEqual("Toto", openWorksheetResult.ExcelWorksheet.Worksheet.Cells[0, 0].StringValue);
			}
		}
	}
}

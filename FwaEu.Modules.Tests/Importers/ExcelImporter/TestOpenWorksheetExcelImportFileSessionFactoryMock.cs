using Aspose.Cells;
using FwaEu.Modules.Importers.ExcelImporter;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.Importers.ExcelImporter
{
	public class TestOpenWorksheetExcelImportFileSessionFactoryMock : IExcelImportFileSessionFactory
	{
		public const string OpenExcelTestWorksheetName = "Test open worksheet";

		public Task<IExcelImportFileSession> CreateExcelImportSessionsAsync(
			ExcelImportContext context, ExcelWorksheet[] worksheets)
		{
			var testOpenWorksheet = worksheets.FirstOrDefault(worksheet => worksheet.Worksheet.Name == OpenExcelTestWorksheetName);

			return Task.FromResult(testOpenWorksheet == null
				? default(IExcelImportFileSession)
				: new TestOpenExcelImportFileSessionMock(context, testOpenWorksheet));
		}
	}
}

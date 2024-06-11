using FwaEu.Modules.Importers.ExcelImporter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.Importers.ExcelImporter
{
	public class TestOpenExcelImportFileSessionMock : IExcelImportFileSession
	{
		private readonly ExcelImportContext _context;
		private readonly ExcelWorksheet _worksheet;

		public TestOpenExcelImportFileSessionMock(ExcelImportContext context, ExcelWorksheet worksheet)
		{
			this._context = context;
			this._worksheet = worksheet;
		}

		public ExcelWorksheet[] OrderedWorksheets => new[] { this._worksheet };

		public Task ImportAsync()
		{
			this._context.Context.ServiceStore.Add<TestOpenExcelImportFileSessionResult>(
				new TestOpenExcelImportFileSessionResult()
				{
					ExcelWorksheet = this._worksheet,
				});

			return Task.CompletedTask;
		}
	}

	public class TestOpenExcelImportFileSessionResult
	{
		public ExcelWorksheet ExcelWorksheet { get; set; }
	}
}

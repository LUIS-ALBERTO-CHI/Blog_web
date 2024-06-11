using FwaEu.Fwamework.Imports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Importers.ExcelImporter
{
	public interface IExcelImportFileSessionFactory
	{
		Task<IExcelImportFileSession> CreateExcelImportSessionsAsync(
			ExcelImportContext context, ExcelWorksheet[] worksheets);
	}

	public interface IExcelImportFileSession
	{
		ExcelWorksheet[] OrderedWorksheets { get; }
		Task ImportAsync();
	}
}

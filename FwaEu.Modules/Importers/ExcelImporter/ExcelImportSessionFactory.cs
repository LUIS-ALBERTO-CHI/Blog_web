using Aspose.Cells;
using FwaEu.Fwamework.Imports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Importers.ExcelImporter
{
	public class ExcelImportSessionFactory : IImportSessionFactory
	{
		public ExcelImportSessionFactory(IEnumerable<IExcelImportFileSessionFactory> excelImportFileSessionFactories)
		{
			this._excelImportFileSessionFactories = excelImportFileSessionFactories;
		}

		private readonly IEnumerable<IExcelImportFileSessionFactory> _excelImportFileSessionFactories;

		private static Workbook GetWorkbook(IImportFile file)
		{
			using (var stream = file.OpenReadStream())
			{
				return new Workbook(stream);
			}
		}

		private static IEnumerable<ExcelWorksheet> GetWorksheets(IImportFile file)
		{
			var workbook = GetWorkbook(file);
			return workbook.Worksheets.Select(worksheet => new ExcelWorksheet(file, worksheet));
		}

		public async Task<IImportFileSession> CreateImportSessionAsync(ImportContext context, IImportFile[] files)
		{
			var excelFiles = files.Where(f => f.Name.EndsWith(".xlsx", StringComparison.InvariantCultureIgnoreCase)
											|| f.Name.EndsWith(".xls", StringComparison.InvariantCultureIgnoreCase)
											|| f.Name.EndsWith(".xlsm", StringComparison.InvariantCultureIgnoreCase)).ToList();

			if (excelFiles.Any())
			{
				var remainingWorksheets = excelFiles.Select(GetWorksheets).SelectMany(w => w).ToList();

				var excelImportContext = new ExcelImportContext(context, remainingWorksheets.ToArray());
				var sessions = new List<IExcelImportFileSession>();
				var handledWorksheets = new List<ExcelWorksheet>(remainingWorksheets.Count);

				foreach (var factory in this._excelImportFileSessionFactories)
				{
					var excelImportSession = await factory.CreateExcelImportSessionsAsync(
						excelImportContext, remainingWorksheets.ToArray());

					if (excelImportSession != null)
					{
						sessions.Add(excelImportSession);
						foreach (var worksheet in excelImportSession.OrderedWorksheets)
						{
							handledWorksheets.Add(worksheet);
							remainingWorksheets.Remove(worksheet);
						}
					}
				}

				if (remainingWorksheets.Any())
				{
					var worksheetsByFiles = String.Join("\n -", remainingWorksheets.ToStringByFile());
					throw new ExcelImporterUnhandledFormatException($"Could not import the following worksheets: \n{worksheetsByFiles}.");
				}

				if (handledWorksheets.Any())
				{
					return new ExcelImportFileSession(sessions.ToArray());
				}
			}

			return default(IImportFileSession);
		}
	}
}

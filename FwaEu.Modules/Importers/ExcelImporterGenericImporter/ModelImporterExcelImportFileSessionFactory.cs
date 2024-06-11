using FwaEu.Modules.Importers.ExcelImporter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Importers.ExcelImporterGenericImporter
{
	public class ModelImporterExcelImportFileSessionFactory : IExcelImportFileSessionFactory
	{
		public ModelImporterExcelImportFileSessionFactory(IServiceProvider serviceProvider)
		{
			this._serviceProvider = serviceProvider;
		}

		private readonly IServiceProvider _serviceProvider;

		public Task<IExcelImportFileSession> CreateExcelImportSessionsAsync(
			ExcelImportContext context, ExcelWorksheet[] worksheets)
		{
			var genericImporterWorksheets = worksheets
				.Where(w => w.Worksheet.Cells[0, 1].StringValue == "GenericImporter")
				.ToArray();

			return Task.FromResult(genericImporterWorksheets.Any()
				? new ModelImporterExcelImportFileSession(this._serviceProvider, context, genericImporterWorksheets)
				: default(IExcelImportFileSession));
		}
	}
}

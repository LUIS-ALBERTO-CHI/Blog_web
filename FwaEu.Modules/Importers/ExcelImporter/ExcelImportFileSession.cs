using FwaEu.Fwamework.Imports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Importers.ExcelImporter
{
	public class ExcelImportFileSession : IImportFileSession
	{
		public ExcelImportFileSession(IExcelImportFileSession[] sessions)
		{
			this.Sessions = sessions ?? throw new ArgumentNullException(nameof(sessions));

			this.OrderedFiles = this.Sessions.SelectMany(
				s => s.OrderedWorksheets.Select(w => w.File))
				.Distinct().ToArray();
		}

		public IExcelImportFileSession[] Sessions { get; }
		public IImportFile[] OrderedFiles { get; }

		public async Task ImportAsync()
		{
			foreach (var session in this.Sessions)
			{
				await session.ImportAsync(); //NOTE: Not using Task.WhenAll() because the execution's order is important
			}
		}
	}
}

using FwaEu.Fwamework.Imports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Importers.ExcelImporter
{
	public class ExcelImportContext
	{
		public ExcelImportContext(ImportContext context, ExcelWorksheet[] worksheets)
		{
			this.Context = context ?? throw new ArgumentNullException(nameof(context));
			this.Worksheets = worksheets ?? throw new ArgumentNullException(nameof(worksheets));
		}

		public ImportContext Context { get; }
		public ExcelWorksheet[] Worksheets { get; }
	}
}

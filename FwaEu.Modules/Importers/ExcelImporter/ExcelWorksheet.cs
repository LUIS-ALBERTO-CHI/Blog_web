using Aspose.Cells;
using FwaEu.Fwamework.Imports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Importers.ExcelImporter
{
	public class ExcelWorksheet
	{
		public ExcelWorksheet(IImportFile file, Worksheet worksheet)
		{
			this.File = file ?? throw new ArgumentNullException(nameof(file));
			this.Worksheet = worksheet ?? throw new ArgumentNullException(nameof(worksheet));
		}

		public IImportFile File { get; }
		public Worksheet Worksheet { get; }
	}
}

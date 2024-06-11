using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Importers.ExcelImporter
{
	public static class ExcelWorksheetExtensions
	{
		public static IEnumerable<string> ToStringByFile(this IEnumerable<ExcelWorksheet> worksheets)
		{
			return worksheets.GroupBy(w => w.File)
				.Select(f =>
				{
					var worksheetNames = String.Join(", ", f.Select(w => w.Worksheet.Name));
					return $"{f.Key.Name}: {worksheetNames}";
				});
		}
	}
}

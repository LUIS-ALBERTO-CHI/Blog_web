using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.GenericImporterWorksheet
{
	public static class WorksheetExtensions
	{
		public static T Read<T>(this Cell cell)
		{
			if (cell.IsEmpty())
			{
				return default(T);
			}

			return (T)Convert.ChangeType(cell.Value, typeof(T));
		}

		public static bool IsEmpty(this Cell cell)
		{
			return cell.Value == null || Object.Equals(cell.Value, String.Empty);
		}
	}
}

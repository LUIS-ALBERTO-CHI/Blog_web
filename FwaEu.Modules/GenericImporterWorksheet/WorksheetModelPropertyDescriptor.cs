using FwaEu.Modules.GenericImporter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.GenericImporterWorksheet
{
	public class WorksheetModelPropertyDescriptor : ModelPropertyDescriptor
	{
		public WorksheetModelPropertyDescriptor(
			string name, IsKeyValue isKey, bool isInfo,
			string[] searchOn, string displayName, int columnIndex)
			: base(name, isKey, isInfo, searchOn, displayName)
		{
			this.ColumnIndex = columnIndex;
		}

		public int ColumnIndex { get; }
	}
}

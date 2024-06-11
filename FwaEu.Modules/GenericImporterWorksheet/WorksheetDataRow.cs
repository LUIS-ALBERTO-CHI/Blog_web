using FwaEu.Modules.GenericImporter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.GenericImporterWorksheet
{
	public class WorksheetDataRow : DataRow
	{
		public WorksheetDataRow(
			WorksheetModelPropertyDescriptor[] worksheetMetadataProperties,
			Dictionary<string, object> valuesByPropertyName, int rowIndex)
			: base(
				  worksheetMetadataProperties.Cast<WorksheetModelPropertyDescriptor>().ToArray(),
				  valuesByPropertyName)
		{
			this.RowIndex = rowIndex;
		}

		public WorksheetModelPropertyDescriptor[] WorksheetMetadataProperties
			() => base.MetadataProperties.Cast<WorksheetModelPropertyDescriptor>().ToArray();

		public int RowIndex { get; }
	}
}

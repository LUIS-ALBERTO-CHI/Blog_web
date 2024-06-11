using Aspose.Cells;
using FwaEu.Modules.GenericImporter;
using FwaEu.Modules.GenericImporterWorksheet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FwaEu.Modules.GenericImporterWorksheet
{
	public class IncludingSchemaWorksheetDataReader : DataReader
	{
		public IncludingSchemaWorksheetDataReader(SchemaDescriptor schema, Worksheet worksheet)
		{
			this._schema = schema ?? throw new ArgumentNullException(nameof(schema));
			this.Worksheet = worksheet ?? throw new ArgumentNullException(nameof(worksheet));
		}

		private readonly SchemaDescriptor _schema;
		public Worksheet Worksheet { get; }

		public override IEnumerable<DataRow> GetRows()
		{
			var schema = this._schema;

			var cells = this.Worksheet.Cells;
			var maxDataRow = cells.MaxDataRow;

			var properties = this.GetProperties()
				.Cast<WorksheetModelPropertyDescriptor>()
				.ToArray();

			for (var dataRowIndex = schema.FirstDataRow; dataRowIndex <= maxDataRow; dataRowIndex++)
			{
				var values = new Dictionary<string, object>();
				foreach (var property in properties)
				{
					var cell = cells[dataRowIndex, property.ColumnIndex];
					var value = cell.IsEmpty() ? default : cell.Value;

					if (property.SearchOn != null && value != null)
					{
						var tuple = ReadSearchOnValue(value);

						if (!property.IsInfo && tuple.ItemCount != property.SearchOn.Length)
						{
							throw new ItemsCountMismatchException(tuple.ItemCount, property.SearchOn.Length);
						}

						value = tuple.Array;
					}

					values.Add(property.Name, value);
				}

				yield return new WorksheetDataRow(properties, values, dataRowIndex);
			}
		}

		private static (Array Array, int ItemCount) ReadSearchOnValue(object value)
		{
			if (value != null)
			{
				if (value is string stringValue)
				{
					var strings = ReadSearchOnMetadata(stringValue);
					return (strings, strings.Length);
				}

				return (new[] { value }, 1);
			}

			return (null, 0);
		}

		private static string[] ReadSearchOnMetadata(string value)
		{
			return String.IsNullOrEmpty(value) ? null : value.Split(',', '|');
		}

		protected override IEnumerable<ModelPropertyDescriptor> LoadProperties()
		{
			var schema = this._schema;
			var cells = this.Worksheet.Cells;
			var maxDataColumn = cells.MaxDataColumn;

			for (var i = schema.FirstDataColumn; i <= maxDataColumn; i++)
			{
				var name = cells[schema.InternalNameRow, i].Read<string>();

				if (String.IsNullOrEmpty(name))
				{
					continue;
				}

				var isKeyCell = cells[schema.IsKeyRow, i];
				var isKey = isKeyCell.IsEmpty()
					? IsKeyValue.False
					: (isKeyCell.Type == CellValueType.IsBool
						? (isKeyCell.Read<bool>() ? IsKeyValue.True : IsKeyValue.False)
						: (IsKeyValue)Enum.Parse(typeof(IsKeyValue), isKeyCell.Read<string>())
						);

				yield return new WorksheetModelPropertyDescriptor(name, isKey,
					cells[schema.IsInfoRow, i].Read<bool>(),
					ReadSearchOnMetadata(cells[schema.SearchOnRow, i].Read<string>()),
					cells[schema.DisplayNameRow, i].Read<string>(), i);
			}
		}
	}

	public class SchemaDescriptor
	{
		public readonly static SchemaDescriptor Default = new SchemaDescriptor(
			modelTypeRow: 1, modelTypeColumn: 1,
			firstDataColumn: 1, firstDataRow: 13, displayNameRow: 12,
			searchOnRow: 10, internalNameRow: 11, isKeyRow: 9, isInfoRow: 8);

		public readonly int? ModelTypeRow;
		public readonly int? ModelTypeColumn;

		public readonly int FirstDataColumn;
		public readonly int FirstDataRow;
		public readonly int DisplayNameRow;
		public readonly int SearchOnRow;
		public readonly int InternalNameRow;
		public readonly int IsKeyRow;
		public readonly int IsInfoRow;

		public SchemaDescriptor(
			int? modelTypeRow, int? modelTypeColumn,
			int firstDataColumn, int firstDataRow,
			int displayNameRow, int searchOnRow,
			int internalNameRow, int isKeyRow, int isInfoRow)
		{
			this.ModelTypeRow = modelTypeRow;
			this.ModelTypeColumn = modelTypeColumn;

			this.FirstDataColumn = firstDataColumn;
			this.FirstDataRow = firstDataRow;
			this.DisplayNameRow = displayNameRow;
			this.SearchOnRow = searchOnRow;
			this.InternalNameRow = internalNameRow;
			this.IsKeyRow = isKeyRow;
			this.IsInfoRow = isInfoRow;
		}
	}

	public class ItemsCountMismatchException : Exception
	{
		public ItemsCountMismatchException(int items, int propertySearchedLength)
			: base($"Items count value " +
				$"<{items}> should match number of " +
				$"items of the SearchOn metadata <{propertySearchedLength}>.")
		{

		}
	}
}

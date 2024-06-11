using FwaEu.Modules.GenericImporter;
using FwaEu.Modules.GenericImporterWorksheet;
using FwaEu.Modules.Importers.ExcelImporter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using FwaEu.Fwamework.Data;

namespace FwaEu.Modules.Importers.ExcelImporterGenericImporter
{
	public class ModelImporterExcelImportFileSession : IExcelImportFileSession
	{
		public ModelImporterExcelImportFileSession(
			IServiceProvider serviceProvider,
			ExcelImportContext excelImportContext,
			ExcelWorksheet[] orderedWorksheets)
		{
			this.ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
			this.ExcelImportContext = excelImportContext ?? throw new ArgumentNullException(nameof(excelImportContext));
			this.OrderedWorksheets = orderedWorksheets ?? throw new ArgumentNullException(nameof(orderedWorksheets));
		}

		protected IServiceProvider ServiceProvider { get; }
		public ExcelImportContext ExcelImportContext { get; }
		public ExcelWorksheet[] OrderedWorksheets { get; }

		protected virtual SchemaDescriptor GetSchema(ExcelWorksheet excelWorksheet)
		{
			return SchemaDescriptor.Default;
		}

		protected virtual Type GetModelType(ExcelWorksheet excelWorksheet, SchemaDescriptor schema)
		{
			if (schema.ModelTypeRow == null || schema.ModelTypeColumn == null)
			{
				throw new NotSupportedException($"The worksheet should provide the model type, or override {nameof(GetModelType)} do to a custom implementation.");
			}

			var rawValue = excelWorksheet.Worksheet.Cells
				[schema.ModelTypeRow.Value, schema.ModelTypeColumn.Value]
				.StringValue;

			var foundType = Type.GetType(rawValue);
			if (foundType == null)
			{
				throw new NotFoundException($"Model type not found: '{rawValue}', you can also override {nameof(GetModelType)} do to a custom type search implementation.");
			}

			return foundType;
		}

		public async Task ImportAsync()
		{
			var context = new ModelImporterContext(
				this.ExcelImportContext.Context.ProcessResult,
				this.ExcelImportContext.Context.ServiceStore);

			foreach (var worksheet in this.OrderedWorksheets)
			{
				var schema = this.GetSchema(worksheet);

				var modelType = this.GetModelType(worksheet, schema);

				var importer = (IModelImporter)this.ServiceProvider.GetRequiredService(
					typeof(IModelImporter<>).MakeGenericType(modelType));

				var reader = new IncludingSchemaWorksheetDataReader(
					schema, worksheet.Worksheet);

				await importer.ImportAsync(reader, context);
			}
		}
	}
}
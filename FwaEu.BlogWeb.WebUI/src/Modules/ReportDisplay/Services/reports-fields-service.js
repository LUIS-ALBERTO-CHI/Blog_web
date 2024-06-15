import DotNetTypesToDevExtremeConverterService from '@UILibrary/Extensions/Services/dot-net-types-to-devextreme-converter-service.js';
import ReportDataSourceService from "@/Modules/Reports/Services/report-data-source-service";
import StringHelperService from '@/Modules/GenericAdmin/Services/string-helper-service';
import ReportFieldMasterDataService from "@/Modules/ReportMasterData/Services/report-field-master-data-service";

export default {
	async getPropertiesAsync(report) {
		const reportFields = await ReportFieldMasterDataService.getByIdsAsync(report.properties
			.filter(p => p.fieldInvariantId)
			.map(p => p.fieldInvariantId));

		const result = [];

		for (const property of report.properties) {
			const fieldName = StringHelperService.lowerFirstCharacter(property.name);
			const p = reportFields.find(rf => rf.invariantId == property.fieldInvariantId) ?? {
				name: fieldName,
				dotNetTypeName: 'String'
			};
			p.fieldName = fieldName;
			result.push(p);
		}

		return result;
	},
	createFieldsGrid(property, viewContext, i18n) {
		let column = { allowHiding: true };
		const field = {
			name: property.fieldName,
			label: property.name,
			cellTemplate: property.templateName,
			urlFormat: property.urlFormat,
			fieldName: property.fieldName,
			format: property.displayFormat
		};

		let dataSource = {
			type: property.dataSourceType,
			argument: property.dataSourceArgument
		};

		if (dataSource.type) {
			column.dataField = field.name;
			column.caption = field.label;
			if (!column.lookup) {
				column.lookup = {};
			}
			column.lookup.dataSource = { load: async () => await ReportDataSourceService.getFieldDataSourceAsync(property.invariantId, dataSource, {}, viewContext) };
			// TODO:Récupérer les displayExpr et valueExpr selon le editorOptions.dataSource ?  https://fwaeu.visualstudio.com/BlogWeb/_workitems/edit/7352				
			column.lookup.displayExpr = 'name';
			column.lookup.valueExpr = 'id';
		}
		else {
			let dotNetTypeToDevExtremeConverter = DotNetTypesToDevExtremeConverterService.getConverter(property.dotNetTypeName);
			column = Object.assign(column, dotNetTypeToDevExtremeConverter.createDataGridColumn(field, i18n));
		}

		return column;
	},

	createFieldsPivot(property, viewContext, i18n) {
		let column = [];
		const field = { name: property.fieldName, label: property.name, summaryType: property.summaryType?.toLowerCase(), format: property.displayFormat };
		let dotNetTypeToDevExtremeConverter = DotNetTypesToDevExtremeConverterService.getConverter(property.dotNetTypeName);
		column = dotNetTypeToDevExtremeConverter.createPivotGridColumn(field, i18n);
		return column;
	}
}

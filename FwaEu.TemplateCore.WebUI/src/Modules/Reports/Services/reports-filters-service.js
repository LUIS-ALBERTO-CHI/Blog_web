import DotNetTypesToDevExtremeConverterService from '@UILibrary/Extensions/Services/dot-net-types-to-devextreme-converter-service.js';
import ReportDataSourceService from "@/Modules/Reports/Services/report-data-source-service";

export default
	{
		getParameterTypes() {
			// NOTE:x.dotNetTypesConverters[0] because we can have many converters for one type
			return DotNetTypesToDevExtremeConverterService.getAll()
				.filter(x => x.createDxFormField)
				.map(function (x) {
					return {
						key: x.dotNetTypesConverters[0].getDisplayName(),
						value: x.dotNetTypesConverters[0].getDotNetTypesHandledForReporting(),
					}
				});
		},

		createFormItem(filter, viewContext) {
			let dxItem = {};
			const field = { name: filter.invariantId, label: filter.name, isRequired: filter.isRequired };
			if (filter.dataSource?.type) {
				dxItem.editorType = 'dxSelectBox';
				dxItem.dataField = field.name;
				dxItem.label = { text: field.label };
				if (!dxItem.editorOptions) {
					dxItem.editorOptions = {};
				}
				dxItem.editorOptions.dataSource = { load: async () => await ReportDataSourceService.getFilterDataSourceAsync(filter.invariantId, filter.dataSource, {}, viewContext) };
				// TODO:Récupérer les displayExpr et valueExpr selon le editorOptions.dataSource ?  https://fwaeu.visualstudio.com/TemplateCore/_workitems/edit/7352				
				dxItem.editorOptions.displayExpr = 'name';
				dxItem.editorOptions.valueExpr = 'id';
			}
			else {
				let dotNetTypeToDevExtremeConverter = DotNetTypesToDevExtremeConverterService.getConverter(filter.dotNetTypeName);
				dxItem = dotNetTypeToDevExtremeConverter.createDxFormField(field);
			}
			return dxItem;
		}
	}
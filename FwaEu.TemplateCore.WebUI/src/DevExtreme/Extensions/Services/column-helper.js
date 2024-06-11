import CustomFieldTypeService from "@UILibrary/Extensions/CellTemplate/Services/custom-field-type-service";

const createCellCustomTemplate = function (container, data, options) {

	let columnType = CustomFieldTypeService.get(options.cellTemplate.toLowerCase());
	if (!columnType)
		throw new Error(`Grid column cell template not found '${options.cellTemplate}'`);
	columnType.createCellCustomTemplateAsync(container, data, options);
}
export default
	{
		createBaseDataGridColumn(options) {
			const columnCellTemplate = options.cellTemplate ? (container, data) => createCellCustomTemplate(container, data, options) : null;
			return {
				dataType: 'string',
				alignmentOverride: undefined, //NOTE: Devextreme default value
				cellTemplate: columnCellTemplate,
				dataField: options.name,
				name: options.name,
				format: options.format,
				width: null,
				formItem: {
					visible: options.isEditable,
					editorOptions:
					{
						showClearButton: !options.isRequired,
						maxLength: options.maxLength,
					}
				},
				editorOptions: {
					format: options.format
				},
				hasValidationRule: true,
				caption: options.label
			};
		},
		createBasePivotGridColumn(options) {
			return {
				dataType: 'string',
				dataField: options.name,
				area: 'filter',
				width: null,
				expanded: true,
				allowExpandAll: true,
				summaryType: options.summaryType,
				caption: options.label,
				format: options.format
			};
		}
	}
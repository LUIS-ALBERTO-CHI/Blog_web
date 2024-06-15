import AbstractDotNetTypeToDevExtremeConverter from './abstract-dot-net-type-to-devextreme-converter';
import DateConverter from '@/Fwamework/DotNetTypeConversion/Services/date-converter';
import ColumnHelper from './column-helper';


class DateToDevExtremeConverter extends AbstractDotNetTypeToDevExtremeConverter
{
	constructor()
	{
		super();
		this.dotNetTypesConverters = [DateConverter];
	}

	createDataGridColumn(options, i18n)
	{
		if (!options.cellTemplate)
			options.cellTemplate = "date";

		let column = ColumnHelper.createBaseDataGridColumn(options);

		column.dataType = 'date';
		
		column.width = 105;
		column.alignment = 'center';
		return column;
	}

	createDxFormField(field) {
		return {
			dataField: field.name,
			label: { text: field.label },
			editorType: 'dxDateBox',
			validationRules: field.isRequired
				? [{ type: "required" }]
				: null
		};
	}

	createPivotGridColumn(options, i18n)
	{
		let field = ColumnHelper.createBasePivotGridColumn(options);
		field.dataType = "date";
		return field;
	}
}

export default new DateToDevExtremeConverter();
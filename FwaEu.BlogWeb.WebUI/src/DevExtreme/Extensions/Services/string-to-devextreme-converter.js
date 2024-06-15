import AbstractDotNetTypeToDevExtremeConverter from './abstract-dot-net-type-to-devextreme-converter';
import StringConverter from '@/Fwamework/DotNetTypeConversion/Services/string-converter';
import ColumnHelper from './column-helper';

class StringToDevExtremeConverter extends AbstractDotNetTypeToDevExtremeConverter {

	constructor() {
		super();
		this.dotNetTypesConverters = [StringConverter];
	}
	createDataGridColumn(options, i18n) {
		return ColumnHelper.createBaseDataGridColumn(options);
	}
	createDxFormField(field) {
		return {
			dataField: field.name,
			label: { text: field.label },
			editorType: 'dxTextBox',
			validationRules: field.isRequired
				? [{ type: "required" }]
				: null
		};
	}
	createPivotGridColumn(options, i18n) {
		return ColumnHelper.createBasePivotGridColumn(options);
	}
}

export default new StringToDevExtremeConverter();
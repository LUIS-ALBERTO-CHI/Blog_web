import AbstractDotNetTypeToDevExtremeConverter from './abstract-dot-net-type-to-devextreme-converter';
import DecimalConverter from '@/Fwamework/DotNetTypeConversion/Services/decimal-converter';
import ColumnHelper from './column-helper';

class DecimalToDevExtremeConverter extends AbstractDotNetTypeToDevExtremeConverter {
	constructor() {
		super();
		this.dotNetTypesConverters = [DecimalConverter];
	}

	createDataGridColumn(options, i18n) {
		let column = ColumnHelper.createBaseDataGridColumn(options);

		column.dataType = "number";
		column.format = column.editorOptions.format = options.format ?? {
			type: "fixedPoint",
			precision: 2,
		};
		return column;
	}

	createDxFormField(field) {
		return {
			dataField: field.name,
			label: { text: field.label },
			editorType: 'dxNumberBox',
			editorOptions: {
				mode: "number",
				format: {
					type: 'fixedPoint',
					precision: 2,
				},
				value: null
			},
			validationRules: field.isRequired
				? [{ type: "required" }]
				: null
		};
	}

	createPivotGridColumn(options, i18n) {
		let field = ColumnHelper.createBasePivotGridColumn(options);
		field.dataType = "number";
		field.format = options.format ?? {
			type: "fixedPoint",
			precision: 2
		}
		return field;
	}
}

export default new DecimalToDevExtremeConverter();
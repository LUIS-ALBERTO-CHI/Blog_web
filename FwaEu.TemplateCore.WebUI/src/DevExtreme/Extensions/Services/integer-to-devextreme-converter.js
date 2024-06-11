import AbstractDotNetTypeToDevExtremeConverter from './abstract-dot-net-type-to-devextreme-converter';
import IntegerConverter from '@/Fwamework/DotNetTypeConversion/Services/integer-converter';
import BigIntegerConverter from '@/Fwamework/DotNetTypeConversion/Services/big-integer-converter';
import ColumnHelper from './column-helper';

class IntegerToDevExtremeConverter extends AbstractDotNetTypeToDevExtremeConverter {
	constructor() {
		super();
		this.dotNetTypesConverters = [IntegerConverter, BigIntegerConverter];
	}

	createDataGridColumn(options, i18n) {
		let column = ColumnHelper.createBaseDataGridColumn(options);
		column.dataType = "number";

		column.format = column.editorOptions.format = options.format ?? {
			type: "fixedPoint",
			precision: 0,
		};

		column.width = 85;
		return column;
	}

	createDxFormField(field) {
		return {
			dataField: field.name,
			label: { text: field.name },
			editorType: 'dxNumberBox',
			editorOptions: {
				mode: "number",
				format: {
					type: 'fixedPoint',
					precision: 0,
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
			precision: 0
		}
		return field;
	}
}

export default new IntegerToDevExtremeConverter();
import AbstractDotNetTypeToDevExtremeConverter from './abstract-dot-net-type-to-devextreme-converter';
import BooleanConverter from '@/Fwamework/DotNetTypeConversion/Services/boolean-converter';
import { I18n } from '@/Fwamework/Culture/Services/localization-service';
import ColumnHelper from './column-helper';

function createSelectBoxEditorOptions() {
	return {
		dataSource: [
			{
				value: true, text: I18n.t('yes')
			},
			{
				value: false, text: I18n.t('no')
			}],
		displayExpr: 'text',
		valueExpr: 'value',
		showClearButton: true,
	}
}

class BooleanToDevExtremeConverter extends AbstractDotNetTypeToDevExtremeConverter {
	constructor() {
		super();
		this.dotNetTypesConverters = [BooleanConverter];
	}

	getDefaultValue(options) {
		if (options.isRequired) {
			return false;
		}
		return null;
	}

	createDataGridColumn(options, i18n) {
		let column = ColumnHelper.createBaseDataGridColumn(options);

		column.dataType = 'boolean';
		column.width = 80;
		column.trueText = I18n.t('yes');
		column.falseText = I18n.t('no');

		if (options.isRequired) {
			column.formItem.editorType = 'dxCheckBox'; //NOTE: Default editor is dxCheckBox but just in case it changes
			column.hasValidationRule = false;
		}
		else {
			column.formItem.editorType = 'dxSelectBox';
			column.formItem.editorOptions = createSelectBoxEditorOptions();
		}
		return column;
	}
	createDxFormField(field) {
		if (field.isRequired) {
			return {
				dataField: field.name,
				label: { text: field.label },
				editorType: 'dxCheckBox'
			};
		}
		return {
			dataField: field.name,
			label: { text: field.label },
			editorType: 'dxSelectBox',
			editorOptions: createSelectBoxEditorOptions(),
			validationRules: field.isRequired
				? [{ type: "required" }]
				: null
		};

	}

	createPivotGridColumn(options, i18n) {
		let field = ColumnHelper.createBasePivotGridColumn(options);
		field.customizeText = function (cellInfo) {
			return I18n.t(cellInfo.value ? 'yes' : 'no');
		}
		return field;
	}
}

export default new BooleanToDevExtremeConverter();
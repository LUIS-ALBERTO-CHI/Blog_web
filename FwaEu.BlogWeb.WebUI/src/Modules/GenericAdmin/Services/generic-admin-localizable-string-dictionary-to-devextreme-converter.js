import AbstractDotNetTypeToDevExtremeConverter from '@UILibrary/Extensions/Services/abstract-dot-net-type-to-devextreme-converter';
import GenericAdminLocalizableStringDictionaryConverter from "@/Modules/GenericAdmin/Services/generic-admin-localizable-string-dictionary-converter";
import ColumnHelper from '@UILibrary/Extensions/Services/column-helper';
import LocalizationService from "@/Fwamework/Culture/Services/localization-service";
import StringHelperService from './string-helper-service';

class GenericAdminLocalizableStringDictionaryToDevExtremeConverter extends AbstractDotNetTypeToDevExtremeConverter {
	constructor() {
		super();
		this.dotNetTypesConverters = [GenericAdminLocalizableStringDictionaryConverter];
	}
	getDefaultValue(options) {
		let defaultValue = {};
		const supportedLanguageCodes = LocalizationService.getSupportedLanguagesCode();
		supportedLanguageCodes.forEach(languageCode => {
			defaultValue[languageCode] = null;
		})
		return defaultValue;
	}

	createDataGridColumn(options, i18n) {
		const supportedLanguageCodes = LocalizationService.getSupportedLanguagesCode();
		let column = ColumnHelper.createBaseDataGridColumn(options);

		if (supportedLanguageCodes.length > 1) {
			column.dataField = undefined;// column is used only as band column so we need to empty data field property
			column.columns = supportedLanguageCodes.map(languageCode => {
				let subColumn = Object.assign({}, column);
				subColumn.formItem = Object.assign({}, column.formItem);
				subColumn.caption = i18n.t(`genericAdminLanguage${StringHelperService.upperFirstCharacter(languageCode)}`);
				subColumn.dataField = `${options.name}.${languageCode}`;
				subColumn.name = subColumn.dataField;
				subColumn.formItem.label = { text: `${column.caption} (${subColumn.caption.toLowerCase()})` }
				subColumn.languageCode = languageCode;
				return subColumn;
			});
		}
		else {
			column.dataField = `${options.name}.${supportedLanguageCodes[0]}`;
			column.languageCode = supportedLanguageCodes[0];
		}
		return column;
	}

	createPivotGridColumn(options, i18n) {
		throw new Error('You must implement the function createPivotGridColumn!');
	}
}

export default new GenericAdminLocalizableStringDictionaryToDevExtremeConverter();
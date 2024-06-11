import AbstractCustomColumnCustomizer from '@/Modules/GenericAdmin/Services/abstract-custom-column-customizer';
import StringHelperService from '@/Modules/GenericAdmin/Services/string-helper-service';

class LocalizableStringColumnCustomizer extends AbstractCustomColumnCustomizer {
	async customizeAsync(columns, properties) {
		await super.customizeAsync(columns, properties);

		for (let column of columns) {
			const property = properties.find(p => StringHelperService.lowerFirstCharacter(p.name) === column.name);
			if (property?.type === "LocalizableString" && column.columns) {
				const supportedLanguageColumns = column.columns.filter(x => property.extendedProperties.supportedCultureCodes.includes(x.languageCode));
				if (supportedLanguageColumns.length === 1) {
					supportedLanguageColumns[0].caption = column.caption;
					columns[columns.indexOf(column)] = supportedLanguageColumns[0];
				}
				else {
					column.columns = supportedLanguageColumns;
				}
			}

		}
	}
}



export default LocalizableStringColumnCustomizer;
import CustomStore from 'devextreme/data/custom_store';
import StringHelperService from '@/Modules/GenericAdmin/Services/string-helper-service';
import DotNetTypesToDevExtremeConverterService from '@UILibrary/Extensions/Services/dot-net-types-to-devextreme-converter-service.js';
import ColumnHelper from '@UILibrary/Extensions/Services/column-helper';

function getValidationRules(options, column) {
	let validationRules = [];

	if (options.isRequired && column.hasValidationRule) {
		validationRules.push({ type: "required" });
	}
	return validationRules;
}

function setPropertyToChildsColumnsIfArray(object, dataKey, value) {
	if (object.columns && Array.isArray(object.columns)) {
		object.columns.forEach(d => setPropertyToChildsColumnsIfArray(d, dataKey, value));
	}
	object[dataKey] = value;
}

export default {
	createColumns(component, i18n) {
		return component.configuration.properties.map(p => {
			const propertyNameCamelCased = StringHelperService.lowerFirstCharacter(p.name); //NOTE: We suppose that the name will be in PascalCasing
			let options = {
				name: propertyNameCamelCased, 
				label: i18n.te(component.configurationKey + '-' + propertyNameCamelCased)
					? i18n.t(component.configurationKey + '-' + propertyNameCamelCased)
					: i18n.t(propertyNameCamelCased),
				isEditable: p.isEditable,
				isRequired: p.extendedProperties.isRequired,
				maxLength: p.extendedProperties.maxLength,
				format: p.format,
				cellTemplate: p.cellTemplate,	
			};
			let dotNetTypeToDevExtremeConverter = DotNetTypesToDevExtremeConverterService.getConverter(p.type);
			let column = dotNetTypeToDevExtremeConverter
				? dotNetTypeToDevExtremeConverter.createDataGridColumn(options, i18n)
				: ColumnHelper.createBaseDataGridColumn(options);

			setPropertyToChildsColumnsIfArray(column, 'validationRules', getValidationRules(options, column));
			column.width = p.width;
			
			return column;
		});
	},
	createCustomStore(storeConfiguration) {
		return new CustomStore({
			key: storeConfiguration.key,
			load: storeConfiguration.load,
			insert: storeConfiguration.insert,
			update: storeConfiguration.update,
			remove: storeConfiguration.remove,
		});
	}
};
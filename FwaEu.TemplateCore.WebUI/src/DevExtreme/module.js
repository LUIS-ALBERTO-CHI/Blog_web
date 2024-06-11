import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';
import './Extensions/Content/dx-styles.scss';
import 'devextreme/dist/css/dx.common.css';
import DotNetTypesToDevExtremeConverterService from './Extensions/Services/dot-net-types-to-devextreme-converter-service.js';
import BooleanConverter from './Extensions/Services/boolean-to-devextreme-converter';
import DateConverter from './Extensions/Services/date-to-devextreme-converter';
import DecimalConverter from './Extensions/Services/decimal-to-devextreme-converter';
import StringConverter from './Extensions/Services/string-to-devextreme-converter';
import IntegerConverter from './Extensions/Services/integer-to-devextreme-converter';
import CustomFieldTypeService from "./Extensions/CellTemplate/Services/custom-field-type-service";
import CustomFieldUrlType from "./Extensions/CellTemplate/Components/custom-field-url-type";
import CustomFieldDateType from "./Extensions/CellTemplate/Components/custom-field-date-type";

export class UILibraryModule extends AbstractModule {
	async onInitAsync() {
		let normalizationService = (await import('./Extensions/Services/normalization-service')).default;
		normalizationService.applyDefaultRules();

		DotNetTypesToDevExtremeConverterService.register(BooleanConverter);
		DotNetTypesToDevExtremeConverterService.register(DateConverter);
		DotNetTypesToDevExtremeConverterService.register(DecimalConverter);
		DotNetTypesToDevExtremeConverterService.register(IntegerConverter);
		DotNetTypesToDevExtremeConverterService.register(StringConverter);
		CustomFieldTypeService.register(CustomFieldUrlType);
		CustomFieldTypeService.register(CustomFieldDateType);
	}
	async onApplicationCreated() {

		let localizationService = (await import('@/Fwamework/Culture/Services/localization-service')).default;
		let culture = localizationService.getCurrentLanguage();
		const devExtremeMessages = import.meta.glob('/node_modules/devExtreme/localization/messages/*.json');
		const deveExtremeCurrentCultureMessages = devExtremeMessages[Object.keys(devExtremeMessages).find(path => path.endsWith(`${culture}.json`))]();
		
		await Promise.all([
			deveExtremeCurrentCultureMessages,
			import('devextreme/localization'),
			import('./Extensions/Services/normalization-service')])
			.then(([messages, localization, normalizationService]) => {
				localization.locale(culture);
				localization.loadMessages(messages),
				normalizationService.default.applyTranslationOverrides();
			});
	}
}
import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';

let localizationService = null;
export class CultureModule extends AbstractModule {
	async onInitAsync(vueApp) {
		localizationService = (await import('@/Fwamework/Culture/Services/localization-service')).default;
		await localizationService.configureAsync(vueApp);

		let httpCultureService = (await import('@/Fwamework/Culture/Services/http-language-headers')).default;
		httpCultureService.configure();

		await localizationService.loadCurrentLanguageAsync();
		await localizationService.getGlobalMessagesAsync();
	}
}


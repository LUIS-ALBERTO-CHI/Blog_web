import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';
import ErrorHandlerService from "@/Fwamework/Setup/Services/setup-error-handler-service";

export class SetupModule extends AbstractModule {
	async onInitAsync(vueApp) {
		ErrorHandlerService.configure(vueApp);
		
		const localizationService = (await import('@/Fwamework/Culture/Services/localization-service')).default;
		const i18n = (await import('@/Fwamework/Culture/Services/localization-service')).I18n;
		await localizationService.configureAsync(vueApp);
		i18n.locale.value = 'en';//NOTE: Force the current locale to 'en' and don't use setCurrentLanguage in order to prevent language change detection that will reload the page

		const authenticationService = (await import('@/Fwamework/Setup/Services/setup-authentication-service')).default;
		if (authenticationService.isAuthenticationEnabled()) {
			const setupAuthenticationService = (await import('./Services/setup-http-authentication')).default;
			setupAuthenticationService.configure();
		}
	}
}
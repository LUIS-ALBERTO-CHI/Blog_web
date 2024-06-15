import HttpService from '@/Fwamework/Core/Services/http-service';

export default {
	configure() {
		this.configureHttpRequest();
	},
	configureHttpRequest() {
		HttpService.interceptors.request.use(async config => {
			let localizationService = (await import('@/Fwamework/Culture/Services/localization-service')).default;
			let currentLanguage = localizationService.getCurrentLanguage();
			config.headers.common['Accept-Language'] = currentLanguage;
			return config;
		});
	}
}
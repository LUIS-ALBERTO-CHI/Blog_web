import HttpService from '@/Fwamework/Core/Services/http-service';
import SetupAuthenticationService from '@/Fwamework/Setup/Services/setup-authentication-service';

export default {
	configure() {
		this.configureHttpRequest();
		this.configureHttpResponse();
	},

	configureHttpRequest() {
		HttpService.interceptors.request.use(async config => {
			const token = SetupAuthenticationService.getCurrentToken();
			config.headers.common.Authorization = `Bearer ${token}`;
			return config;
		});
	},

	configureHttpResponse() {
		HttpService.interceptors.response.use(response => response, async (error) => {
			if (error.response && error.response.status === 401) {
				error.isHandled = true;
				SetupAuthenticationService.logout();
			}
			return Promise.reject(error);
		});
	}
}
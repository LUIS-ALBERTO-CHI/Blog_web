import HttpService from '@/Fwamework/Core/Services/http-service';
import NotificationService from '@/Fwamework/Notifications/Services/notification-service';
import { I18n } from '@/Fwamework/Culture/Services/localization-service';

export default {
	configure() {
		this.configureHttpResponse();
	},

	configureHttpResponse() {
		HttpService.interceptors.response.use(response => response, async (error) => {
			if (error.response && error.response.status === 403) {
				NotificationService.showError(I18n.t("errorMessage"));
				error.isHandled = true;
			}
			return Promise.reject(error);
		});
	}
}
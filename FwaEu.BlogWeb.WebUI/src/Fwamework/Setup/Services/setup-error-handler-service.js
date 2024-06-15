const GetNotificationService = () => import('@/Fwamework/Notifications/Services/notification-service').then(serv => serv.default);

export default {
	configure(vueApp) {
		vueApp.config.errorHandler = function (error) {
			if (error) {
				let responseMesage = error.isAxiosError ? error.response.data.message : error.reason?.response?.data?.message;
				let message = responseMesage || error.message || error.reason.message;

				GetNotificationService().then(serv => serv.showError(message));
			}
		};
		window.addEventListener('unhandledrejection', vueApp.config.errorHandler);
	}
};
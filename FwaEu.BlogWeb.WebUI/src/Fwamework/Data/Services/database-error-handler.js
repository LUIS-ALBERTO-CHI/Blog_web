const conflictStatusCode = 409;

export default {
	exportName: 'ErrorHandler',
	key: 'DatabaseErrorHandler',
	async onErrorAsync(error) {
		const ajaxResponse = error?.response;
		if (ajaxResponse && ajaxResponse.status === conflictStatusCode && ajaxResponse.data?.indexOf("DbConstraint") !== -1) {
			const notificationService = (await import('@/Fwamework/Notifications/Services/notification-service')).default;
			const i18n = (await import('@/Fwamework/Culture/Services/localization-service')).I18n;

			error.isHandled = true;
			notificationService.showError(i18n.t(ajaxResponse.data));
		}
	}
};

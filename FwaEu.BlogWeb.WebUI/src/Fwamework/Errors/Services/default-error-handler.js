export default {
	exportName: 'ErrorHandler',
	key: 'DefaultErrorHandler',
	async onErrorAsync(error) {
		error.isHandled = true;
		let message = error.message;
		const ajaxResponse = error?.response;
		if (ajaxResponse) {
			await handleAjaxErrorAsync(ajaxResponse, error.message);
		}
		else {
			
			const notificationService = (await import('@/Fwamework/Notifications/Services/notification-service')).default;
			notificationService.showError(message);
		}
	}
};

const codesWithDefaultMessages = [400, 401, 403, 404];
async function handleAjaxErrorAsync(response, errorMessage) {
	const i18n = (await import('@/Fwamework/Culture/Services/localization-service')).I18n;
	
	let message = getMessageFromResponse(response, errorMessage, i18n);
	if (response.status >= 400 && response.status < 500) {
		const notificationService = (await import('@/Fwamework/Notifications/Services/notification-service')).default;
		notificationService.showError(message);
	}
	else {
		const dialogService = (await import('@UILibrary/Modules/Dialog/Services/dialog-service')).default;
		dialogService.alertAsync(i18n.t('errorMessage5xx'), i18n.t('unknownErrorMessage'));
	}
}

function getMessageFromResponse(response, errorMessage, i18n) {

	if (codesWithDefaultMessages.includes(response.status)) {
		return i18n.t(`errorMessage${response.status}`);
	} else {
		//NOTE: Error message can be contained in diferent properties depending on error
		let message = response.data?.title ??
			errorMessage ??
			response.data?.message ??
			response.exceptionMessage ??
			i18n.t('unknownErrorMessage');

		if (response.statusText) {
			message = `${message} (${response.statusText})`;
		}
		return message;
	}
	
}
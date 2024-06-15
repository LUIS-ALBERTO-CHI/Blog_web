import { LocalStorage } from '@/Fwamework/Storage/Services/local-storage-store';
import ErrorHandlerService from '@/Fwamework/Errors/Services/error-handler-service';

const versionCheckDelayInMilieconds = 60 * 60 * 1000;//1h minutes
let needToCheckVersion = true;

export default {

	async configureAsync(vueApplication, applicationConfiguration) {

		await showNotificationIfNewVersionAsync();

		//Start interval for version update checks
		setInterval(() => { needToCheckVersion = true; }, versionCheckDelayInMilieconds);
		const Router = (await import('@/Fwamework/Routing/Services/vue-router-service')).default;

		Router.beforeEach(async (to, from, next) => {
			if (needToCheckVersion) {
				needToCheckVersion = false;
				await checkNewVersionAsync(applicationConfiguration);
			}
			next();
		});

		ErrorHandlerService.registerErrorHandler({
			onErrorAsync(error) {
				
				const ajaxResponse = error.response || error.reason?.response;
				if (ajaxResponse && ajaxResponse.status === 500) {
					needToCheckVersion = true;
				}
			}
		});
	}
}

async function checkNewVersionAsync(currentConfiguration) {

	const serverMonitoringService = (await import('@/Modules/ServerMonitoring/Services/server-monitoring-service')).default;
	const applicationInfoService = (await import('@/Fwamework/Core/Services/application-info-service')).default;
	const newApplicationInfo = await serverMonitoringService.getApplicationInfoAsync();
	const currentApplicationInfo = applicationInfoService.get();
	const applicationUrl = currentConfiguration.application.publicUrl;

	if (newApplicationInfo && newApplicationInfo.version !== currentApplicationInfo.version) {
		const i18n = (await import('@/Fwamework/Culture/Services/localization-service')).I18n;
		LocalStorage.setValue("newVersionMessage", i18n.t("newVersionMessage")); //Save the update version message
		window.location = applicationUrl;
	}
}

async function showNotificationIfNewVersionAsync() {
	let newVersionMessage = LocalStorage.getValue("newVersionMessage");
	if (newVersionMessage) {
		const notificationService = (await import('@/Fwamework/Notifications/Services/notification-service')).default;
		notificationService.showInformation(newVersionMessage, { timeout: 5000 });
		LocalStorage.removeValue("newVersionMessage");
	}
}


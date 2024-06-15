import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";
import { Configuration } from "@/Fwamework/Core/Services/configuration-service";
import localizationService, { I18n } from "@/Fwamework/Culture/Services/localization-service";
import serverMonitoringService from "./Services/server-monitoring-service";

export class ServerMonitoringModule extends AbstractModule {


	/** @param {{checkForUpdates: boolean }} options */
	constructor(options = null) {
		super();
		this.options = options ?? { checkForUpdates: true };
	}

	async onInitAsync(vueApp) {
		await serverMonitoringService.pingAsync().catch(reason => {
			if (!reason.status) {

				reason.isHandled = true;
				const offlineMessage = document.getElementById("serverOfflineMessage");

				if (offlineMessage) {
					localizationService.loadCurrentLanguageAsync().then(() => {
						localizationService.loadGlobalMessagesAsync([import(`./Content/server-monitoring-global-messages.${localizationService.getCurrentLanguage()}.json`)]).then(() => {
							offlineMessage.innerText = I18n.t("serverOfflineMessage");
							offlineMessage.style.display = 'flex';
							document.getElementById("reloadLink").innerText = I18n.t("reloadApplicationText");
							document.getElementById("reloadLink").style.display = 'block';
						});
					});
				}
				else {
					alert(I18n.t("serverOfflineMessage"));
				}
				document.getElementById("loaderIcon").setAttribute("src", "error.gif");
			}
		});
	}

	async onApplicationCreated(vueInstance) {
		if (this.options.checkForUpdates) {
			const UpdateCheckerService = (await import("./Services/update-checker-service")).default;
			UpdateCheckerService.configureAsync(vueInstance, Configuration);
		}
	}
}

import AbstractModule  from "@/Fwamework/Core/Services/abstract-module-class";
import UserNotificationService from "@/Modules/UserNotifications/Services/user-notification-service"
import masterDataManagerService from "@/Fwamework/MasterData/Services/master-data-manager-service";

const masterDataNotificationType = "MasterDataChanged";

export class MasterDataNotificationsModule extends AbstractModule {

	onInitAsync() {

		UserNotificationService.onNotified(async (e) => {
			if (e.notificationType === masterDataNotificationType) {
				await masterDataManagerService.clearCacheAsync(e.content.model);
			}
		});
	}
}

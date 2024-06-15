import AbstractModule  from "@/Fwamework/Core/Services/abstract-module-class";
import UserNotificationService from "@/Modules/UserNotifications/Services/user-notification-service"
import UserTasksManagerService from "@/Modules/UserTasks/Services/user-tasks-manager-service";

const userTaskUserNotificationType = "UserTask";

export class UserTasksUserNotificationsModule extends AbstractModule {

	onInitAsync() {

		UserNotificationService.onNotified(async (e) => {
			if (e.notificationType === userTaskUserNotificationType) {
				await UserTasksManagerService.handleUserTaskUpdateAsync(e.content.model.userTaskName, e.content.model.result);
			}
		});
	}
}

import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";
import UserTasksManagerService from "@/Modules/UserTasks/Services/user-tasks-manager-service";
import UserTaskNotificationDisplayZoneHandler from "./Service/user-task-display-zone-handler";

export class UserTasksNotificationModule extends AbstractModule {

	async onInitAsync() {
		UserTasksManagerService.addDisplayZoneHandler(UserTaskNotificationDisplayZoneHandler);
	}
}
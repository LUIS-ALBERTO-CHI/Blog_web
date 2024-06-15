import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";
import UserTasksManagerService from "@/Modules/UserTasks/Services/user-tasks-manager-service";
import UserTaskListDisplayZoneHandler from "./Services/user-task-display-zone-handler";

export class UserTasksListModule extends AbstractModule {

	async onInitAsync() {
		UserTasksManagerService.addDisplayZoneHandler(UserTaskListDisplayZoneHandler);
	}
}
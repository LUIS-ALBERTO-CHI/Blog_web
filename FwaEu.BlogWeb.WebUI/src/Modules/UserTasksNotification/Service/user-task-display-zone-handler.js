import { UserTaskDisplayZoneHandler } from "@/Modules/UserTasks/Services/user-task-display-zone-handler";
import NotificationService from "@/Fwamework/Notifications/Services/notification-service";

export const NotificationDisplayZoneName = "notification";

/** @typedef {import("@/Modules/UserTasks/Services/user-task").UserTask} UserTask */

export class UserTaskNotificationDisplayZoneHandler extends UserTaskDisplayZoneHandler {
	constructor() {
		super(NotificationDisplayZoneName);
	}

	/** @param {UserTask} userTask  @returns {Promise}*/
	async handleUserTaskUpdateAsync(userTask) {

		const newUserTaskValue = await userTask.getValueAsync(this.zoneName);
		NotificationService.showInformation(newUserTaskValue);
	}
}

export default new UserTaskNotificationDisplayZoneHandler();
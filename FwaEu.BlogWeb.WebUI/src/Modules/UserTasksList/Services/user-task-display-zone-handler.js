import { UserTaskDisplayZoneHandler } from "@/Modules/UserTasks/Services/user-task-display-zone-handler";

export const ListDisplayZoneName = "list";

/** @typedef {import("@/Modules/UserTasks/Services/user-task").UserTask} UserTask */

export class UserTaskListDisplayZoneHandler extends UserTaskDisplayZoneHandler {
	constructor() {
		super(ListDisplayZoneName);
	}

	async handleUserTaskUpdateAsync(userTask){
		//NOTE: Nothing to do
	}
}

export default new UserTaskListDisplayZoneHandler();
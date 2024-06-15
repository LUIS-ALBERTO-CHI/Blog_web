import UserTasksManagerService from "./user-tasks-manager-service";

/** @typedef {import("./user-task").UserTask} UserTask */

export class UserTaskDisplayZoneHandler {
	constructor(zoneName) {
		this.zoneName = zoneName;
	}

	/** @param {UserTask} userTask @returns {Promise}*/
	handleUserTaskUpdateAsync(userTask) {
		throw new Error("You must implement handleUserTaskUpdateAsync");
	}

	/** @returns {Promise<Array<UserTask>>} */
	async getAllTasksAsync() {
		const allTasks = await UserTasksManagerService.getAllAsync(this.zoneName);
		return allTasks;
	}
}
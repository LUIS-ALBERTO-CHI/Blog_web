import UserTasksService from "./user-tasks-service";

/** @typedef {import("./user-task").UserTask} UserTask */
export default {
	/** @type {Array<UserTask>} */
	_userTasks: [],

	_userTaskDisplayZoneHandlers: [],

	add(userTask) {
		this._userTasks.push(userTask);
	},

	addDisplayZoneHandler(userTask) {
		this._userTaskDisplayZoneHandlers.push(userTask);
	},

	removeDisplayZoneHandler(userTask) {
		this._userTaskDisplayZoneHandlers.splice(this._userTaskDisplayZoneHandlers.indexOf(userTask), 1);
	},

	async getAllAsync(zoneName) {
		const allUserTaskNames = await UserTasksService.getAccessibleTasksAsync();
		let allUserTasks = this._userTasks.filter(ut => allUserTaskNames.includes(ut.name));

		if (zoneName)
			allUserTasks = allUserTasks.filter(ut => ut.displayZones.includes(zoneName));

		return allUserTasks;
	},

	async handleUserTaskUpdateAsync(userTaskName, parameters) {
		const userTask = this._userTasks.find(x => x.name === userTaskName);
		if (!userTask)
			throw new Error(`Missing user task for '${userTaskName}'`);

		const allUserTaskNames = await UserTasksService.getAccessibleTasksAsync();
		if (!allUserTaskNames.includes(userTask.name))
			return;

		await userTask.handleChangeAsync(parameters);
		const handlerUpdatePromises = [];
		for (const displayZone of userTask.displayZones) {
			const displayZoneHandler = this._userTaskDisplayZoneHandlers.find(handler => handler.zoneName === displayZone);
			if (displayZoneHandler)
				handlerUpdatePromises.push(displayZoneHandler.handleUserTaskUpdateAsync(userTask));
		}

		await Promise.all(handlerUpdatePromises);
	}
}

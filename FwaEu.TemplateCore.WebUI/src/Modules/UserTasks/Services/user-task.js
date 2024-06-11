import AsyncQueue from "@/Fwamework/Concurrency/async-queue";
import userTasksService from "./user-tasks-service";

export class UserTask {

	/**@param {String} name
	 * @param {Array<String>} displayZones
	 */
	constructor(name, displayZones) {
		if (!name)
			throw new Error("Missing required argument userTaskName");

		if (!displayZones || displayZones.lenght === 0)
			throw new Error("You must provide at least one display zone");

		this.name = name;
		this.displayZones = displayZones;
		this._currentValue = null;
		this._valueLoaderQueue = new AsyncQueue(1, true);
	}

	async handleChangeAsync(changeParameters) {
		//NOTE: By default we only replace the current user task value but custom implementations can perform other extra actions if required
		//for exemple you can save the update parameters and use them for the next getValueAsync call or simply mark your task as obsolete and handle the reload within getValueAsync
		this._currentValue = changeParameters;
		return this;
	}

	//NOTE: Each user task can override this function and append contextual data
	async getParametersAsync() {
		return {};
	}

	async getValueAsync(displayZoneName, forceReload = false) {
		if (!this._currentValue || forceReload) {
			const parameters = await this.getParametersAsync();
			const result = await this.executeAsync(parameters);
			this._currentValue = result;
		}
		return this.createDisplayZoneModel(displayZoneName, this._currentValue);
	}

    async executeAsync(parameters) {
        return await this._valueLoaderQueue.runAsync(async () => await userTasksService.executeAsync(this.name, parameters));
    }

	createDisplayZoneModel(zoneName, value) {
		return value;
	}
}
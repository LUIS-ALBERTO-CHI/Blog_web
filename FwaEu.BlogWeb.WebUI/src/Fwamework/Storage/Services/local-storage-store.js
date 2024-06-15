/* eslint-disable @fwaeu/custom-rules/no-local-storage */
import Store from "./abstract-store";
import { Configuration } from "@/Fwamework/Core/Services/configuration-service";

class LocalStorageStore extends Store {

	/**
	 * @param {String} keyPrefix
	 */
	constructor(keyPrefix = "") {
		super();
		this.keyPrefix = keyPrefix;
	}

	setValue(cacheKey, objValue) {
		localStorage.setItem(this.getItemKey(cacheKey), JSON.stringify(objValue));
	}

	getValue(cacheKey) {
		return JSON.parse(localStorage.getItem(this.getItemKey(cacheKey)));
	}

	removeValue(cacheKey) {
		localStorage.removeItem(this.getItemKey(cacheKey));
	}

	//Implement base store functions
	async setValueAsync(cacheKey, objValue) { this.setValue(cacheKey, objValue); }

	async getValueAsync(cacheKey) { return this.getValue(cacheKey); }

	async removeValueAsync(cacheKey) { this.removeValue(cacheKey); }

	getItemKey(cacheKey) {
		return Configuration.application.technicalName + "_" + this.keyPrefix + "_" + cacheKey;
	}
}
export default LocalStorageStore;

export const LocalStorage = new LocalStorageStore();

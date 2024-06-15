import Store from "./abstract-store";

class SessionStorageStore extends Store {

	/**
	 * @param {String} keyPrefix
	 */
	constructor(keyPrefix = "") {
		super();
		this.keyPrefix = keyPrefix;
	}

	setValue(cacheKey, objValue) {
		sessionStorage.setItem(this.getItemKey(cacheKey), JSON.stringify(objValue));
	}

	getValue(cacheKey) {
		return JSON.parse(sessionStorage.getItem(this.getItemKey(cacheKey)));
	}

	removeValue(cacheKey) {
		sessionStorage.removeItem(this.getItemKey(cacheKey));
	}

	//Implement base store functions
	async setValueAsync(cacheKey, objValue) { this.setValue(cacheKey, objValue); }

	async getValueAsync(cacheKey) { return this.getValue(cacheKey); }

	async removeValueAsync(cacheKey) { this.removeValue(cacheKey); }

	getItemKey(cacheKey) {
		return this.keyPrefix + "_" + cacheKey;
	}
}
export default SessionStorageStore;

export const SessionStorage = new SessionStorageStore();

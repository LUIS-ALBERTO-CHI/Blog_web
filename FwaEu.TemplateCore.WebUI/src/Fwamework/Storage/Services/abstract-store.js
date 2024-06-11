export default class Store {

	/**
	 * @param {String} cacheKey
	 * @param {any} objValue
	 * @returns {Promise<void>}
	 */
	setValueAsync(cacheKey, objValue) {
		throw new Error("setValueAsync must be implemented");
	}

	/**
	 * @param {String} cacheKey
	 * @returns {Promise<any>}
	 */
	getValueAsync(cacheKey) {
		throw new Error("getValueAsync must be implemented");
	}

	/**
	 * @param {String} cacheKey
	 * @returns {Promise<void>}
	 */
	removeValueAsync(cacheKey) {
		throw new Error("removeValueAsync must be implemented");
	}
}
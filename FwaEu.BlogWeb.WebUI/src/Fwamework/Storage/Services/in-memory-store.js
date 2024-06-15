import Store from "./abstract-store";

class InMemoryStore extends Store {
	storedItems = new Map();

	setValue(cacheKey, objValue) {
		this.storedItems.set(cacheKey, objValue);
	}

	getValue(cacheKey) {
		let cacheItem = this.storedItems.get(cacheKey);
		return (cacheItem != undefined) ? cacheItem : null;
	}

	removeValue(cacheKey) {
		this.storedItems.delete(cacheKey);
	}

	//Implement base store functions
	async setValueAsync(cacheKey, objValue) { this.setValue(cacheKey, objValue); }

	async getValueAsync(cacheKey) { return this.getValue(cacheKey); }

	async removeValueAsync(cacheKey) { this.removeValue(cacheKey); }
}

export default InMemoryStore;
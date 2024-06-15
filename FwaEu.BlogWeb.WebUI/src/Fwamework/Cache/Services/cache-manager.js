const upToDateStatus = 'UpToDate';
const expiredStatus = 'Expired';
const pendingChangesStatus = 'PendingChanges';
const allowedItemStatus = [upToDateStatus, expiredStatus, pendingChangesStatus];

class CacheManager {
	constructor(store) {
		if (!store.setValueAsync || !store.getValueAsync || !store.removeValueAsync)
			throw new Error("Invalid store: setValueAsync, getValueAsync and removeValueAsync functions are required");

		this.store = store;
	}

	cacheItemsConfig = [];
	store = null;

	/**
	 * 
	 * @param {any} cacheKey
	 * @param {any} options expirationDelay (in seconds) + expirationCallbackAsync
	 */
	async configureItemAsync(cacheKey, options) {

		let cacheItem = {
			key: cacheKey,
			unsavedValue: null,
			options: options
		};
		this.cacheItemsConfig.push(cacheItem);
		let status = null;

		let storedValue = await this.store.getValueAsync(cacheKey);
		if (storedValue) {
			var expiredDate = this.calculateDateOfExpiry(storedValue.updatedOn, options);
			if (expiredDate) {
				status = new Date() > expiredDate ? expiredStatus : upToDateStatus;
			} else {
				status = expiredStatus;
			}
			await this.forceStatusAsync(cacheKey, status);
		}
		return status;
	}

	async getItemValueAsync(cacheKey) {
		let storedValue = await this.store.getValueAsync(cacheKey);
		return storedValue ? storedValue.value : null;
	}

	updateValue(cacheKey, value) {
		let cacheItem = this.cacheItemsConfig.find(x => x.key === cacheKey);
		cacheItem.unsavedValue = value;
		cacheItem.status = pendingChangesStatus;
		return this;
	}

	async saveChangesAsync() {
		let pendingItemsConfig = this.cacheItemsConfig.filter(x => x.status === pendingChangesStatus);

		for (const cacheItem of pendingItemsConfig) {
			let storedValue = { value: cacheItem.unsavedValue, updatedOn: new Date() };
			await this.store.setValueAsync(cacheItem.key, storedValue);
			cacheItem.status = upToDateStatus;
			cacheItem.unsavedValue = null;
			await this.reinitializeTimerAsync(cacheItem, storedValue);
		}
	}

	getStatus(cacheKey) {
		let cacheItem = this.cacheItemsConfig.find(x => x.key === cacheKey);
		return cacheItem ? cacheItem.status : null;
	}

	async removeItemAsync(cacheKey) {
		let cacheItem = this.cacheItemsConfig.find(x => x.key === cacheKey);
		if (cacheItem) {
			if (cacheItem.timer) {
				clearTimeout(cacheItem.timer);
			}
			await this.store.removeValueAsync(cacheKey);
			this.cacheItemsConfig.slice(this.cacheItemsConfig.indexOf(cacheItem), 1);
		}
	}

	async forceStatusAsync(cacheKey, cacheStatus) {
		if (allowedItemStatus.indexOf(cacheStatus) === -1)
			throw new Error(`Invalid cache status: ${cacheStatus}, allowed statuses are: ${allowedItemStatus.join(', ')}`);

		let cacheItem = this.cacheItemsConfig.find(x => x.key === cacheKey);
		if (!cacheItem)
			throw new Error("Not found cache item with key: " + cacheKey);
		let previousStatus = cacheItem.status;
		cacheItem.status = cacheStatus;

		if (cacheStatus === expiredStatus) {
			clearTimeout(cacheItem.timer);
			if (cacheItem.expirationCallbackAsync)
				await cacheItem.expirationCallbackAsync(cacheItem);
		}
		else if (cacheStatus === upToDateStatus) {
			if (previousStatus === pendingChangesStatus) {
				cacheItem.unsavedValue = null;
			}

			let storedValue = await this.store.getValueAsync(cacheItem.key);
			storedValue.updatedOn = new Date();
			await this.store.setValueAsync(cacheItem.key, storedValue);
			await this.reinitializeTimerAsync(cacheItem, storedValue);
		}
	}

	calculateDateOfExpiry(updatedOn, options) {
		if (updatedOn) {
			updatedOn = new Date(updatedOn);
			var expiredDate = new Date(updatedOn.setSeconds(updatedOn.getSeconds() + options.expirationDelayInSeconds));
			return expiredDate;
		}
		return null;
	}

	async createTimeoutCallbackAsync(item, storedValue) {
		var expiredDate = this.calculateDateOfExpiry(storedValue.updatedOn, item.options);
		let remainingTimeBeforeExpiration = expiredDate - new Date();
		if (!expiredDate || remainingTimeBeforeExpiration <= 0) {
			if (item.options.expirationCallbackAsync) {
				await item.options.expirationCallbackAsync(item);
			}
			return null;
		}
		else {
			return setTimeout(async () => {
				item.status = expiredStatus;
				if (item.options.expirationCallbackAsync) {
					await item.options.expirationCallbackAsync(item);
				}
			}, remainingTimeBeforeExpiration);
		}
	}

	async reinitializeTimerAsync(item, storedValue) {
		clearTimeout(item.timer);
		item.timer = await this.createTimeoutCallbackAsync(item, storedValue);
	}
}

export default CacheManager
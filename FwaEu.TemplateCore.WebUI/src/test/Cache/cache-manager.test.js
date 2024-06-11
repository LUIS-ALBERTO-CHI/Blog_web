/**
 * @vitest-environment jsdom
 */
import InMemoryStore from '@/Fwamework/Storage/Services/in-memory-store';
import CacheManager from '@/Fwamework/Cache/Services/cache-manager.js';
import { describe, expect, it } from 'vitest';

describe('Cache manager tests', function () {
	it("Testing Cache manager when status is expired (null updatedOn value)", async () => {
		let inMemoryStoreInstance = new InMemoryStore();
		await inMemoryStoreInstance.setValueAsync('storedItemKey', {
			value: 'storedItemValue', updatedOn: null
		});
		var options = {
			expirationDelayInSeconds: 15, expirationCallbackAsync:
				function () { }
		};
		let cacheManager = new CacheManager(inMemoryStoreInstance);
		let itemStatus = await cacheManager.configureItemAsync('storedItemKey', options);
		expect(itemStatus).toEqual('Expired');
	})

	it("Testing Cache manager when vlaue is not stored", async () => {
		let inMemoryStoreInstance = new InMemoryStore();
		var options = {
			expirationDelayInSeconds: 15, expirationCallbackAsync:
				function () { }
		};
		let cacheManager = new CacheManager(inMemoryStoreInstance);
		let itemStatus = await cacheManager.configureItemAsync('storedItemKey', options);
		expect(itemStatus).toEqual(null);
	});

	it("Testing Cache manager when status is updated (existing item)", async () => {
		let inMemoryStoreInstance = new InMemoryStore();
		await inMemoryStoreInstance.setValueAsync('storedItemKey', {
			value: 'storedItemValue', updatedOn: new Date()
		});
		var options = {
			expirationDelayInSeconds: 24 * 60 * 60, expirationCallbackAsync:
				function () { }
		};
		let cacheManager = new CacheManager(inMemoryStoreInstance);
		let itemStatus = await cacheManager.configureItemAsync('storedItemKey', options);
		expect(itemStatus).toEqual('UpToDate');
	});

	it("Testing Cache manager when status is updated (new item)", async () => {
		let inMemoryStoreInstance = new InMemoryStore();
		var options = {
			expirationDelayInSeconds: 24 * 60 * 60,
			expirationCallbackAsync:
				function () { }
		};
		let cacheManager = new CacheManager(inMemoryStoreInstance);
		await cacheManager.configureItemAsync('storedItemKey', options);
		await cacheManager.updateValue('storedItemKey', "NewValue").saveChangesAsync();

		let itemStatus = cacheManager.getStatus('storedItemKey');

		expect(itemStatus).toEqual('UpToDate');
	});

	it("Testing Cache manager when status is expired (duration expired)", async () => {
		let inMemoryStoreInstance = new InMemoryStore();
		let expirationDelay = 24 * 60 * 60;
		let storedValueUpdatedOn = new Date();
		storedValueUpdatedOn.setSeconds(storedValueUpdatedOn.getSeconds() - expirationDelay - 1);

		await inMemoryStoreInstance.setValueAsync('storedItemKey', {
			value: 'storedItemValue', updatedOn: storedValueUpdatedOn
		});
		var options = {
			expirationDelayInSeconds: expirationDelay, expirationCallbackAsync:
				function () { }
		};
		let cacheManager = new CacheManager(inMemoryStoreInstance);
		await cacheManager.configureItemAsync('storedItemKey', options);
		let status = cacheManager.getStatus('storedItemKey');
		expect(status).toEqual('Expired');
	});
});
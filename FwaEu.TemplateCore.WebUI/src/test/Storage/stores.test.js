/**
 * @vitest-environment jsdom
 */
import Store from '@/Fwamework/Storage/Services/abstract-store';
import InMemoryStore from '@/Fwamework/Storage/Services/in-memory-store';
import LocalStorageStore from '@/Fwamework/Storage/Services/local-storage-store';
import { describe, expect, it } from 'vitest';
describe('Store tests', function () {
	const stores = [new InMemoryStore(), new LocalStorageStore()];
	it("Testing store wtih simple type", async () => {
		const itemValue = 5;
		for (var store of stores) {
			console.log(`Testing ${store.constructor.name}`);

			await store.setValueAsync('ItemCacheKey', itemValue);
			let data = await store.getValueAsync('ItemCacheKey');
			expect(data).toStrictEqual(itemValue);

			await store.removeValueAsync('ItemCacheKey');

			data = await store.getValueAsync('ItemCacheKey');
			expect(data).toEqual(null);
		}
	});

	it("Testing store with object", async () => {
		const itemValue = { prop1: { sub: "sub" }, prop2: 5 };
		for (var store of stores) {
			console.log(`Testing ${store.constructor.name}`);

			await store.setValueAsync('ItemCacheKey', itemValue);
			let data = await store.getValueAsync('ItemCacheKey');

			expect(data).toStrictEqual(itemValue);

			await store.removeValueAsync('ItemCacheKey');

			data = await store.getValueAsync('ItemCacheKey');
			expect(data).toEqual(null);
		}
	});


	class NotImplementedStore extends Store {

	}

	it("Test not implemented Store throw error", async function () {
		console.log("Testing NotImplementedStore");
		expect.assertions(3);

		const store = new NotImplementedStore();
		let setValueError = null;
		try {
			await store.setValueAsync("ItemCacheKey", {});
		} catch (e) {
			setValueError = e;
		}
		expect(setValueError).toEqual(new Error("setValueAsync must be implemented"));

		let getValueError = null;
		try {
			await store.getValueAsync("ItemCacheKey");
		} catch (e) {
			getValueError = e;
		}
		expect(getValueError).toEqual(new Error("getValueAsync must be implemented"));

		let removeValueError = null;
		try {
			await store.removeValueAsync("ItemCacheKey");
		} catch (e) {
			removeValueError = e;
		}
		expect(removeValueError).toEqual(new Error("removeValueAsync must be implemented"));
	});
});
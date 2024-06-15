import IndexedDbService from '@/Modules/IndexedDb/Services/indexed-db-service';
import Store from '@/Fwamework/Storage/Services/abstract-store';
import MasterDataManagerService, { getMasterDataChangeInfoKey, getMasterDataItemByKeyStoreKey } from "@/Fwamework/MasterData/Services/master-data-manager-service";

const DatabaseVersion = 1;
const DatabaseName = "masterData";

async function openMasterDataDataBaseAsync(databaseName, databaseVersion) {
	return await IndexedDbService.openAsync(databaseName, databaseVersion, async db => {
		MasterDataManagerService._masterDataRegistry.forEach(md => {
			if (!db.objectStoreNames.contains(md.configuration.masterDataKey)) {
				db.createObjectStore(md.configuration.masterDataKey, { keyPath: "__id", autoIncrement: true });
				db.createObjectStore(getMasterDataChangeInfoKey(md.configuration.masterDataKey), { keyPath: "__id", autoIncrement: true });
				db.createObjectStore(getMasterDataItemByKeyStoreKey(md), { keyPath: "__id", autoIncrement: true });
			}
		});
	});
}

/**
 * @param {String} cacheKey
 */
const isChangeInfoAsync = async (cacheKey) => {
	const ChangeInfoStoreKeySuffix = (await import("@/Fwamework/MasterData/Services/master-data-manager-service")).ChangeInfoStoreKeySuffix;
	return cacheKey.endsWith(ChangeInfoStoreKeySuffix);
};

class IndexedDbMasterDataStore extends Store {

	/**
	 * @param {String} cacheKey
	 * @param {Array<any>} objValue
	 * @returns {Promise<void>}
	 */
	async setValueAsync(cacheKey, objValue) {
		return new Promise(async (resolve, reject) => {
			if (objValue == null)
				return;

			const isChangeInfo = await isChangeInfoAsync(cacheKey);
			const dataToSave = isChangeInfo ? [objValue] : objValue;
			const db = await openMasterDataDataBaseAsync(DatabaseName, DatabaseVersion);

			const transaction = db.transaction(cacheKey, 'readwrite');
			transaction.oncomplete = resolve;
			transaction.onerror = reject;

			const store = transaction.objectStore(cacheKey);
			for (const data of dataToSave) {
				// NOTE: put returns an IDBRequest, which holds events and is asynchronous, no need to await it,
				// the transaction will trigger oncomplete when all requests are done.
				store.put(data);
			}
		});
	}

	/**
	 * @param {String} cacheKey
	 * @returns {Promise<any>}
	 */
	async getValueAsync(cacheKey) {
		return new Promise(async (resolve, reject) => {
			const isChangeInfo = await isChangeInfoAsync(cacheKey);
			const db = await openMasterDataDataBaseAsync(DatabaseName, DatabaseVersion);
			const itemsRequest = db.transaction(cacheKey, 'readonly')
				.objectStore(cacheKey)
				.getAll();

			itemsRequest.onsuccess = (es) => {
				db.close();
				resolve(isChangeInfo ? es.target.result[0] : es.target.result);
			};

			itemsRequest.onerror = (er) => {
				db.close();
				reject(er);
			};
		});
	}


	/**
	 * @param {String} cacheKey
	 * @returns {Promise<void>}
	 */
	async removeValueAsync(cacheKey) {
		await new Promise(async (resolve, reject) => {
			const db = await openMasterDataDataBaseAsync(DatabaseName, DatabaseVersion);

			const transaction = db.transaction(cacheKey, 'readwrite');
			transaction.objectStore(cacheKey).clear();

			transaction.oncomplete = resolve;
			transaction.onerror = reject;
		});
		db.close();
	}
	
}

export default IndexedDbMasterDataStore;
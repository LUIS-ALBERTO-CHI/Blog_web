import { Configuration } from "@/Fwamework/Core/Services/configuration-service";

/**
 * @type {IDBFactory}
 * */
const indexedDB = window.indexedDB || window.mozIndexedDB || window.webkitIndexedDB || window.msIndexedDB;
let firstOpenLock = {};

export default {
	_internalOpenAsync(databaseName, databaseVersion, upgradeDatabase) {
		const request = indexedDB.open(databaseName, databaseVersion);

		return new Promise((resolve, reject) => {
			request.onerror = reject;
			request.onsuccess = (event) => resolve(event.target.result);
			request.onupgradeneeded = (event) => upgradeDatabase(event.target.result);
		});
	},

	/**
	 * @param {String} databaseName
	 * @param {Number} databaseVersion
	 * @param {(dbUpdate: IDBDatabase) => any | (dbUpdate: IDBDatabase) => Promise<any>} upgradeDatabase
	 * @returns {IDBDatabase}
	 */
	async openAsync(databaseName, databaseVersion, upgradeDatabase) {
		if (!firstOpenLock[databaseName]) {

			firstOpenLock[databaseName] = this._internalOpenAsync(this.getDatabaseName(databaseName), databaseVersion, upgradeDatabase);
			return await firstOpenLock[databaseName];
		}
		else {
			await firstOpenLock[databaseName];
		}
		return await this._internalOpenAsync(this.getDatabaseName(databaseName), databaseVersion, upgradeDatabase);
	},

	getDatabaseName(databaseName) {
		return Configuration.application.technicalName + "_" + databaseName;
	},

	async deleteDatabaseAsync(databaseName) {
		return new Promise((resolve, reject) => {
			const request = indexedDB.deleteDatabase(this.getDatabaseName(databaseName));
			request.onsuccess = resolve;
			request.onerror = reject;
		});
	}
}
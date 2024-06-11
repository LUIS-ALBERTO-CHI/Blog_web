import MasterDataManagerService from "@/Fwamework/MasterData/Services/master-data-manager-service";

export class MasterDataService {

	constructor(masterDataKey, keyFields, fullLoad = true) {
		if (!masterDataKey)
			throw new Error("Missing required parameter masterDataKey");
		if (!keyFields || keyFields.length === 0)
			throw new Error("Missing required parameter keyFields");

		this.configuration = new MasterDataConfiguration(masterDataKey, keyFields, fullLoad, null);
	}

	async configureAsync() {
		await MasterDataManagerService.configureMasterDataAsync(this);
	}

	async getAllAsync() {
		return await MasterDataManagerService.getMasterDataAsync(this.configuration.masterDataKey);
	}

	async getAllRemoteAsync(parameters) {
		const result = await MasterDataManagerService.getRemoteMasterDataAsync(this.configuration.masterDataKey, parameters);
		
		return result;
	}

	async getAsync(key) {
		const [result] = await this.getByIdsAsync([key]);
		return result;
	}

	async getByIdsAsync(keys) {
		return await MasterDataManagerService.getMasterDataByKeysAsync(this.configuration.masterDataKey, keys);
	}

	async clearCacheAsync() {
		await MasterDataManagerService.clearCacheAsync(this.configuration.masterDataKey);
	}

	createItem(item) {
		return {
			...item,
			toString() {
				return this.name || this.text || this;//By default try to display name and text fields
			}
		}
	}
}


export class MasterDataConfiguration {

	/**
	 * @typedef {import('@/Fwamework/MasterData/Services/custom-data-loader-service').CustomDataLoaderService} DataLoaderService
	 * @typedef {import('@/Fwamework/Storage/Services/abstract-store').default} Store
	 * @param {String} masterDataKey
	 * @param {Array<String>} keyFields
	 * @param {Boolean} fullLoad
	 * @param {DefaultMasterDataItemKeyHelper} keyHelper
	 * @param {DataLoaderService} customDataLoaderService
	 * @param {Store} customStore
	 */
	constructor(masterDataKey, keyFields, fullLoad, keyHelper = null, customDataLoaderService = null, customStore = null) {

		this.masterDataKey = masterDataKey;
		this.keyFields = keyFields;
		this.fullLoad = fullLoad;
		this.customDataLoaderService = customDataLoaderService;
		this.customStore = customStore;

		this.keyHelper = keyHelper || new DefaultMasterDataItemKeyHelper(keyFields);
	}

	getFetchDataParameters() {
		return { masterDataKey: this.masterDataKey };
	}
}

export class DefaultMasterDataItemKeyHelper {

	/** @param { Array<String> } keyFields */
	constructor(keyFields) {
		const singleKey = keyFields.length === 1;

		this.equals = function (firstItemKey, secondItemKey) {
			return singleKey ? firstItemKey == secondItemKey : keyFields.every(field => firstItemKey[field] == secondItemKey[field]);
		};

		this.isValid = function (itemKey) {
			return singleKey ? itemKey !== null && itemKey !== undefined : keyFields.some(field => itemKey[field] !== null && itemKey[field] !== undefined);
		};

		this.serialize = function (itemKey) {
			return JSON.stringify(itemKey);
		}

		this.getItemKey = function (item) {
			if (item === null || item === undefined)
				return null;
			return singleKey ? item[keyFields[0]] : keyFields.map(field => item[field]);
		}
	}
}


export default MasterDataService;
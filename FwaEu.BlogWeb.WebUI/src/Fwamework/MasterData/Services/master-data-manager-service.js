import { Configuration } from "@/Fwamework/Core/Services/configuration-service";
import DefaultMasterDataService from "./default-data-loader-service";


/**
* @typedef {import('@/Fwamework/MasterData/Services/master-data-service').MasterDataService} MasterDataService
* @typedef {import('@/Fwamework/MasterData/Services/master-data-service').MasterDataConfiguration} MasterDataConfiguration
* @typedef {import('@/Fwamework/MasterData/Services/default-data-loader-service').MasterDataDataLoaderService} MasterDataDataLoaderService
* @typedef {import('@/Fwamework/Storage/Services/abstract-store').default} Store
* @typedef {{masterDataKey: String, maximumUpdatedOn: Date, count: Number}} ChangeInfo
*/
export const ChangeInfoStoreKeySuffix = '_ChangeInfo';
export const ItemByKeyStoreKeySuffix = '_ItemsByKey';


export function getMasterDataChangeInfoKey(masterDataKey) {
	return masterDataKey + ChangeInfoStoreKeySuffix;
}
/** @param {MasterDataService} masterDataService
 * @param {any} itemKey
 * @returns {String}
 */
export function getMasterDataItemByKeyStoreKey(masterDataService) {
	return `${masterDataService.configuration.masterDataKey}${ItemByKeyStoreKeySuffix}`;
}

class MasterDataManagerService {

	constructor() {

		/** @type {Store} */
		this._defaultStore = null;

		/** @type {Array<MasterDataService>} */
		this._masterDataRegistry = [];

		this._refreshTimer = new MasterDataRefreshTimer();
	}

	/**@param {MasterDataService} masterDataService
	 * @returns {Promise} */
	async configureMasterDataAsync(masterDataService) {
		this._masterDataRegistry.push(masterDataService);
	}

	/**@param {String} masterDataKey
	 * @returns {MasterDataService} */
	getMasterDataService(masterDataKey) {
		const masterDataService = this._masterDataRegistry.find(md => md.configuration.masterDataKey === masterDataKey);
		if (!masterDataService)
			throw new Error(`'${masterDataKey}' is not registered, you must register the master data by calling the 'configureMasterDataAsync' function`);
		return masterDataService;
	}

	/** @returns {Array<MasterDataService>} */
	getAllMasterDataServices() {
		return this._masterDataRegistry;
	}

	/**@param {String} masterDataKey 
	 * @returns {Promise<Array>} */
	async getMasterDataAsync(masterDataKey) {
		const masterDataService = this.getMasterDataService(masterDataKey);

		/** @type {Array} */
		let storedData = await this._getStore(masterDataService).getValueAsync(masterDataKey);

		//If no stored data was found, then we need to fetch it
		if (!storedData?.length) {
			storedData = await this._firstMasterDataLoadAsync(masterDataService);
		}

		return storedData.map(item => masterDataService.createItem(item));
	}

	/**Fetch master data information without cache management, used for remote operations processing like search and pagination
	 * @param {String} masterDataKey 
	 * @param {String} parameters
	 * @returns {Promise<Array>} */
	async getRemoteMasterDataAsync(masterDataKey, parameters) {
		const masterDataService = this.getMasterDataService(masterDataKey);
		let fetchedMasterData = [];

		if (!masterDataService.configuration.customDataLoaderService)
			fetchedMasterData = (await DefaultMasterDataService.getMasterDataAsync([{ masterDataKey: masterDataKey, ...parameters }]))[0].values;
		else
			fetchedMasterData = (await masterDataService.configuration.customDataLoaderService.getMasterDataAsync(parameters)).values;
		return fetchedMasterData.map(item => masterDataService.createItem(item));
	}

	/**@param {String} masterDataKey
	 * @param {Array} keys
	 * @returns {Promise<Array<any>>} */
	async getMasterDataByKeysAsync(masterDataKey, keys) {

		const masterDataService = this.getMasterDataService(masterDataKey);
		const keyHelper = masterDataService.configuration.keyHelper;

		let keysToFetch = [];
		for (const key of keys) {
			//Remove invalid and duplicate keys
			if (keyHelper.isValid(key) && !keysToFetch.some(keyToFetch => keyHelper.equals(keyToFetch, key)))
				keysToFetch.push(key);
		}

		let result = [];

		//Try to load from full master data cache
		/**@type {Array} */
		let fullMasterDataCache = await this._getStore(masterDataService).getValueAsync(masterDataKey);

		if (masterDataService.configuration.fullLoad && !fullMasterDataCache)
			fullMasterDataCache = await this._firstMasterDataLoadAsync(masterDataService);

		if (fullMasterDataCache) {
			const foundCachedItems = fullMasterDataCache.filter(x => keysToFetch.some(key => keyHelper.equals(key, keyHelper.getItemKey(x))));
			keysToFetch = keysToFetch.filter(key => !foundCachedItems.some(loadedItem => keyHelper.equals(keyHelper.getItemKey(loadedItem), key)));
			result = result.concat(foundCachedItems);
		}

		if (keysToFetch.length > 0) {

			const cachedItemsByKey = await this._getStore(masterDataService).getValueAsync(getMasterDataItemByKeyStoreKey(masterDataService));
			if (cachedItemsByKey && cachedItemsByKey.length > 0) {
				//Try to load from by key master data cache
				const foundCachedItemsByKey = cachedItemsByKey.filter(x => keysToFetch.some(key => keyHelper.equals(key, keyHelper.getItemKey(x))));
				keysToFetch = keysToFetch.filter(key => !foundCachedItemsByKey.some(loadedItem => keyHelper.equals(keyHelper.getItemKey(loadedItem), key)));
				result = result.concat(foundCachedItemsByKey);
			}

			//If keys are missing then load and save them into the cache by key
			if (keysToFetch.length > 0) {
				const masterDataResult = await this._getMasterDataByKeysAsync(masterDataService, masterDataKey, keysToFetch);
				await this._updateCacheAsync(masterDataService, masterDataKey, masterDataResult.remoteMasterDataChangeInfo, masterDataResult.newItemsLoadedByKeys);
				result = result.concat(masterDataResult.newItemsLoadedByKeys);
			}
		}
		return result.map(item => masterDataService.createItem(item));
	}

	async refreshMasterDataIfNeededAsync() {
		if (this._refreshTimer.isExpired) {
			await this._refreshAllMasterDatasAsync();
			this._refreshTimer.reset();
		}
	}

	/**Performs the first master data fetch and saves the fetched data into the store
	 * @param {MasterDataService} masterDataService 
	 * @returns {Promise<Array>} */
	async _firstMasterDataLoadAsync(masterDataService) {

		const [fetchedMasterData] = (await this._loadMasterDatasAsync([masterDataService], true));
		let result = [];

		//Because of parallel calls and _loadMasterDatasAsync only returns a result if the data is not up to date, sometimes we will give an empty result
		if (!fetchedMasterData) {
			result = await this._getStore(masterDataService).getValueAsync(masterDataService.configuration.masterDataKey);
		} else {
			result = fetchedMasterData.values;
			//Because it is the first fetch, we directly save the new data into the store
			await this._getStore(masterDataService).setValueAsync(masterDataService.configuration.masterDataKey, result);
		}

		this._refreshTimer.start();
		return result;
	}

	/**
	 * This function will refresh all master data at the same time
	 */
	async _refreshAllMasterDatasAsync() {

		//Only reload already loaded master datas
		const localMasterDatasChangeInfo = await this.getLocalChangesInfoAsync(this._masterDataRegistry);
		const masterDataServices = this._masterDataRegistry.filter(md => localMasterDatasChangeInfo.find(ci => ci.masterDataKey === md.configuration.masterDataKey));

		if (masterDataServices.length > 0) {

			//Clear full loaded master data caches
			const fullLoadMasterDataServices = masterDataServices.filter(md => md.configuration.fullLoad);
			const fullMasterDataResults = await this._loadMasterDatasAsync(fullLoadMasterDataServices, false);

			//Save the new fetched data
			for (const newMasterData of fullMasterDataResults) {
				await this._getStore(newMasterData.service).setValueAsync(newMasterData.service.configuration.masterDataKey, newMasterData.values);
				await this._getStore(newMasterData.service).setValueAsync(getMasterDataItemByKeyStoreKey(newMasterData.service), []);//Clear items cache by key
			}

			//Clear not full loaded master data by key cache
			let notFullLoadMasterDataServices = masterDataServices.filter(md => !md.configuration.fullLoad);
			const remoteMasterDatasChangeInfo = await this.getRemoteChangesInfoAsync(notFullLoadMasterDataServices);
			notFullLoadMasterDataServices = await this._getOutdatedMasterDataServicesAsync(notFullLoadMasterDataServices, remoteMasterDatasChangeInfo);

			for (const masterDataService of notFullLoadMasterDataServices) {
				await this._getStore(masterDataService).setValueAsync(getMasterDataItemByKeyStoreKey(masterDataService), []);//Clear items cache by key
			}
		}
	}

	/**@param {Array<MasterDataService>} masterDataServices
	 * @param {boolean} ignoreUpToDateVerification
	* @returns {Promise<Array<MasterDataLoadResult>>} */
	async _loadMasterDatasAsync(masterDataServices, ignoreUpToDateVerification) {

		const remoteMasterDatasChangeInfo = await this.getRemoteChangesInfoAsync(masterDataServices);

		if (!ignoreUpToDateVerification) {
			//Include only outdated master datas
			masterDataServices = await this._getOutdatedMasterDataServicesAsync(masterDataServices, remoteMasterDatasChangeInfo);
			if (masterDataServices.length === 0) {
				return [];
			}
		}

		const fetchedMasterDatas = await this._fetchMasterDatasAsync(masterDataServices);

		let fullMasterDataResults = [];
		for (const fetchedMasterData of fetchedMasterDatas) {
			const masterDataKey = fetchedMasterData.masterDataKey;
			const changeInfo = remoteMasterDatasChangeInfo.find(ci => ci.masterDataKey === masterDataKey);
			const masterDataService = this.getMasterDataService(masterDataKey);

			//Update the date change information
			await this._getStore(masterDataService).setValueAsync(getMasterDataChangeInfoKey(masterDataKey), changeInfo);
			fullMasterDataResults.push(new MasterDataLoadResult(masterDataService, fetchedMasterData.values));
		}
		return fullMasterDataResults;
	}


	/** @typedef {{ masterDataKey: String, values: Array}} FetchMasterDataResult
	 *  @param {Array<MasterDataService>} masterDataServices
	 *  @returns {Promise<Array<FetchMasterDataResult>> */
	async _fetchMasterDatasAsync(masterDataServices) {
		let defaultMasterDataServices = masterDataServices.filter(md => !md.configuration.customDataLoaderService);
		let customMasterDataServices = masterDataServices.filter(md => md.configuration.customDataLoaderService);

		const masterDataFetchParameters = await Promise.all(masterDataServices.map(md => md.configuration.getFetchDataParameters()));

		/** @type {Array<FetchMasterDataResult>} */
		let fetchedMasterDatas = [];
		if (defaultMasterDataServices.length > 0) {
			fetchedMasterDatas = await DefaultMasterDataService.getMasterDataAsync(defaultMasterDataServices.map(s => masterDataFetchParameters.find(p => p.masterDataKey == s.configuration.masterDataKey)));
		}
		if (customMasterDataServices.length > 0) {
			fetchedMasterDatas = fetchedMasterDatas.concat(
				await Promise.all(customMasterDataServices.map(s => s.configuration.customDataLoaderService.getMasterDataAsync(masterDataFetchParameters.find(p => p.masterDataKey == s.configuration.masterDataKey))
					.then(result => ({ masterDataKey: s.configuration.masterDataKey, ...result }))))
			);
		}
		return fetchedMasterDatas;
	}

	/** @param {Array<MasterDataService>} masterDataServices
	 *  @param {Promise<Array<ChangeInfo>> remoteMasterDatasChangeInfo
	 *  @returns {Promise<Array<MasterDataService>> */
	async _getOutdatedMasterDataServicesAsync(masterDataServices, remoteMasterDatasChangeInfo) {
		const localMasterDatasChangeInfo = await this.getLocalChangesInfoAsync(masterDataServices);

		masterDataServices = masterDataServices.filter(md => {
			const localChangeInfo = localMasterDatasChangeInfo.find(ci => ci.masterDataKey === md.configuration.masterDataKey);
			const remoteChangeInfo = remoteMasterDatasChangeInfo.find(ci => ci.masterDataKey === md.configuration.masterDataKey);

			return !localChangeInfo
				|| localChangeInfo.maximumUpdatedOn < remoteChangeInfo.maximumUpdatedOn
				|| localChangeInfo.count !== remoteChangeInfo.count;
		});
		return masterDataServices;
	}

	/** @param {Array<MasterDataService>} masterDataServices
	 *  @returns {Promise<Array<ChangeInfo>> */
	async getRemoteChangesInfoAsync(masterDataServices) {

		let defaultMasterDataServices = masterDataServices.filter(md => !md.configuration.customDataLoaderService);
		let customMasterDataServices = masterDataServices.filter(md => md.configuration.customDataLoaderService);
		let remoteMasterDatasChangeInfo = [];
		const defaultMasterDataKeys = defaultMasterDataServices.map(md => md.configuration.masterDataKey);

		if (defaultMasterDataKeys.length > 0)
			remoteMasterDatasChangeInfo = await DefaultMasterDataService.getMasterDataChangeInfoAsync(defaultMasterDataKeys);

		if (customMasterDataServices.length > 0) {
			remoteMasterDatasChangeInfo = remoteMasterDatasChangeInfo.concat(await Promise.all(customMasterDataServices.map(function (service) {
				return service.configuration.customDataLoaderService.getMasterDataChangeInfoAsync(service.configuration.masterDataKey)
					.then(result => ({ masterDataKey: service.configuration.masterDataKey, ...result }));
			})));
		}
		return remoteMasterDatasChangeInfo;
	}

	async _getMasterDataByKeysAsync(masterDataService, masterDataKey, keysToFetch) {
		const [remoteMasterDataChangeInfo] = await this.getRemoteChangesInfoAsync([masterDataService]);
		let newItemsLoadedByKeys;

		if (!masterDataService.configuration.customDataLoaderService) {
			const getByKeysParameter = { masterDataKey: masterDataKey, keys: keysToFetch };
			const [fetchedMasterData] = await DefaultMasterDataService.getMasterDataByKeysAsync([getByKeysParameter]);
			newItemsLoadedByKeys = fetchedMasterData.values;
		}
		else {
			const fetchedMasterData = await masterDataService.configuration.customDataLoaderService.getMasterDataByKeysAsync(keysToFetch);
			newItemsLoadedByKeys = fetchedMasterData.values;
		}

		return { remoteMasterDataChangeInfo, newItemsLoadedByKeys };
	}

	/**@param {Array<MasterDataService>} masterDataServices
	 * @returns {Promise<Array<ChangeInfo>}
	 */
	async getLocalChangesInfoAsync(masterDataServices) {
		/**
		 * @type {Array<ChangeInfo>}
		 * */
		let changeInfo = [];
		for (let masterDataService of masterDataServices) {

			const storedChangeInfo = await this._getStore(masterDataService).getValueAsync(getMasterDataChangeInfoKey(masterDataService.configuration.masterDataKey));
			if (storedChangeInfo) {
				changeInfo.push(storedChangeInfo);
			}
		}
		return changeInfo;
	}

	/** @param {MasterDataService} masterDataService
	 * @returns {Store}	 */
	_getStore(masterDataService) {
		return masterDataService.configuration.customStore || this._defaultStore;
	}

	async _updateCacheAsync(masterDataService, masterDataKey, remoteMasterDataChangeInfo, newItemsLoadedByKeys) {
		//Update change info information and update cache by key
		await this._getStore(masterDataService).setValueAsync(getMasterDataChangeInfoKey(masterDataKey), remoteMasterDataChangeInfo);
		const cachedItemsByKey = await this._getStore(masterDataService).getValueAsync(getMasterDataItemByKeyStoreKey(masterDataService));
		await this._getStore(masterDataService).setValueAsync(getMasterDataItemByKeyStoreKey(masterDataService), (cachedItemsByKey ?? []).concat(newItemsLoadedByKeys));
	}

	/** @param {Array<String>} masterDataKeys If not provided, all master data caches will be cleared*/
	async clearCacheAsync(masterDataKeys = null) {
		const masterDataServicesToClear = masterDataKeys ? this._masterDataRegistry.filter(md => masterDataKeys.includes(md.configuration.masterDataKey)) : this._masterDataRegistry;

		for (const masterDataService of masterDataServicesToClear) {
			const masterDataKey = masterDataService.configuration.masterDataKey;
			const masterDataStore = this._getStore(masterDataService);

			await masterDataStore.removeValueAsync(getMasterDataChangeInfoKey(masterDataKey));
			await masterDataStore.removeValueAsync(getMasterDataItemByKeyStoreKey(masterDataService));
			await masterDataStore.removeValueAsync(masterDataKey);

		}
	}
}

class MasterDataRefreshTimer {

	constructor() {
		this.cacheDurationInSeconds = Configuration.fwamework.masterData.cache.checkPeriodicityInSeconds;
		this.timeoutId;
	}


	start() {
		if (!this.timeoutId) {
			const $this = this;
			this.isExpired = false;
			this.timeoutId = setTimeout(() => {
				$this.isExpired = true;
			}, this.cacheDurationInSeconds * 1000);
		}
	}

	reset() {
		clearTimeout(this.timeoutId);
		this.timeoutId = null;
		this.start();
	}
}

class MasterDataLoadResult {
	/**
	 * @param {MasterDataService} service
	 * @param {Array} values 
	 */
	constructor(service, values) {
		this.service = service;
		this.values = values;
	}
}

export default new MasterDataManagerService();

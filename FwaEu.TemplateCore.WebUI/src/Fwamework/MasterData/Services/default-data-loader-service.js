import HttpService from '@/Fwamework/Core/Services/http-service';

export class DefaultDataLoaderService {

	/**
	* @typedef {Array<{ propertyName: String, ascending: Boolean }>} OrderByParameters
	* @typedef {{skip: Number, take: Number}} PaginationParameters
	* @param {Array<{masterDataKey: String, search: String, pagination: PaginationParameters, orderBy: OrderByParameters }>} masterDataParameters 
	* @returns {Promise<Array<{ masterDataKey: String, values: Array}>>}
	*/
	async getMasterDataAsync(masterDataParameters) {
		const result = await HttpService.post('MasterData/GetModels', masterDataParameters);
		return result.data;
	}

	/**
	* @param {Array<{masterDataKey: String, keys: Array}>} masterDataParameters
	* @returns {Promise<Array<{ masterDataKey: String, values: Array}>>}
	*/
	async getMasterDataByKeysAsync(masterDataParameters) {
		const getModesByIdsRequest = masterDataParameters.map(param => ({ masterDataKey: param.masterDataKey, ids: param.keys }));
		const result = await HttpService.post('MasterData/GetModelsByIds', getModesByIdsRequest);
		return result.data;
	}

	/**
	 * @param {Array<String>} masterDataKeys
	 * @returns {Promise<Array<{ masterDataKey: String, count: Number, maximumUpdatedOn: Date}>>}
	 */
	async getMasterDataChangeInfoAsync(masterDataKeys) {
		const masterDataParameters = masterDataKeys.map(key => { return { masterDataKey: key }; });
		const result = await HttpService.post('MasterData/GetChangeInfos', masterDataParameters);
		result.data.forEach(changeInfo => {
			changeInfo.maximumUpdatedOn = new Date(changeInfo.maximumUpdatedOn);//Force to have a Date value instead of string
		});

		return result.data;
	}
}


export default new DefaultDataLoaderService();
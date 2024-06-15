/** Signature for custom master data loader service */
export class CustomDataLoaderService {
	/**
	* @typedef {Array<{ propertyName: String, ascending: Boolean }>} OrderByParameters
	* @typedef {{skip: Number, take: Number}} PaginationParameters
	* @param {{search: String, pagination: PaginationParameters, orderBy: OrderByParameters }} masterDataParameters
	* @returns {Promise<{ values: Array}>}
	*/
	async getMasterDataAsync(masterDataParameters) {
		throw new Error('You must implement the getMasterDataAsync function');
	}

	/**
	* @param {Array} keys
	* @returns {Promise<{ values: Array}>}
	*/
	async getMasterDataByKeysAsync(keys) {
		throw new Error('You must implement the getMasterDataByKeysAsync function');
	}

	/**
	 * @param {String} masterDataKey
	 * @returns {Promise<{ count: Number, maximumUpdatedOn: Date}>}
	 */
	async getMasterDataChangeInfoAsync(masterDataKey) {
		throw new Error('You must implement the getMasterDataChangeInfoAsync function');
	}
}
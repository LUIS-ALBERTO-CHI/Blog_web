import CustomStore from "devextreme/data/custom_store";
import DataSourceHelper from "./data-source-helper";

export default {

	/**
	 * @param {import("@/Fwamework/MasterData/Services/master-data-service").default} masterDataService
	 * @param {import('devextreme/data/custom_store').CustomStoreOptions} options
	 * @returns {import("devextreme/data/data_source").DataSourceOptions}
	 */
	create(masterDataService, options) {

		let dataSourceOptions = {};

		dataSourceOptions.masterDataService = masterDataService;
		const remoteOperations = !masterDataService.configuration.fullLoad;
		const masterDataKeyFields = masterDataService.configuration.keyFields;

		//Set the default paging options if remote operations are enabled
		if (remoteOperations) {
			dataSourceOptions.pageSize = 30;
			dataSourceOptions.paginate = true;
		}

		let storeOptions = Object.assign({
			key: masterDataKeyFields.length === 1 ? masterDataKeyFields[0] : masterDataKeyFields,
			loadMode: remoteOperations ? 'processed' : 'raw',
			cacheRawData: false, //NOTE: cache is managed by masterdata
			load: async function (loadOptions) {
				if (!remoteOperations) {
					return await dataSourceOptions.masterDataService.getAllAsync();
				}

				//HACK: DevExteme DataGrid doesn't support async loading of items to be displayed
				//We need to return an empty reslult for the first call if remote pagination is enabled in order to prevent full data to be fetched
				//The following code needs to be used with a DataGrid custom data source that handles preload items by id
				//Search: 18664f34-6fcc-4d8b-8daf-207b9338cbcc
				if (dataSourceOptions.paginate && !loadOptions.take) {

					//NOTE: DxTagBox uses load instead of byKey function to retrieve the selected items from .value property https://supportcenter.devexpress.com/ticket/details/bc4240/tagbox-now-uses-the-datasource-load-method-with-a-filter-instead-of-the-bykey-datasource
					if (loadOptions.filter)
						console.warn(`MasterData ${masterDataService.configuration.masterDataKey} - DataSourceOptions: Filter is not supported here, if you are using DxTagBox please implement your own load solution or enable fullLoad option`);

					return [];
				}

				const parameters = DataSourceHelper.getMasterDataParametersFromLoadOptions(loadOptions);

				return await dataSourceOptions.masterDataService.getAllRemoteAsync(parameters);
			},
			//NOTE: If not remote operations, then we must leave the default DX implementation for byKey function
			byKey: !remoteOperations ? undefined : async function (key) {
				return await dataSourceOptions.masterDataService.getAsync(key);
			}
		}, options);

		dataSourceOptions.store = new CustomStore(storeOptions);

		dataSourceOptions.store.byKeys = async function (keys) {
			return await dataSourceOptions.masterDataService.getByIdsAsync(keys);
		};
		return dataSourceOptions;
	}
}

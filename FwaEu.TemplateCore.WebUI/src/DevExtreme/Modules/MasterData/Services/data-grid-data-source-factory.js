import CustomStore from "devextreme/data/custom_store";
import DataSource from "devextreme/data/data_source";

/**
* @typedef { import('devextreme/data/data_source').DataSourceOptions & { getDataGrid: () =>  import('devextreme-vue').DxDataGrid, storeOptions: import('devextreme/data/custom_store').CustomStoreOptions  } } DataGridDataSourceOptions
*/

export default {

	/**
	 * @param {DataGridDataSourceOptions} dataSourceOptions
	 * @returns {DataSource}
	 */
	createDataSource(dataSourceOptions) {

		if (!dataSourceOptions)
			throw new Error("Missing required dataSourceOptions argument");

		if (!dataSourceOptions.getDataGrid)
			throw new Error("Missing required dataSourceOptions.getDataGrid argument");

		if (!dataSourceOptions.storeOptions)
			throw new Error("Missing required dataSourceOptions.storeOptions argument");

		if (!dataSourceOptions.storeOptions.load)
			throw new Error("Missing required dataSourceOptions.storeOptions.load function");

		const innerLoadFunction = dataSourceOptions.storeOptions.load;
		dataSourceOptions.storeOptions.load = async function (options) {
			const loadedItems = await innerLoadFunction(options);//Load data source
			await preloadColumnsDataAsync(dataSourceOptions, loadedItems?.data ?? loadedItems);

			return loadedItems;
		};

		//If loadMode is processed we need to intercept the byKey function call
		if (dataSourceOptions.storeOptions.loadMode === "processed") {

			if (!dataSourceOptions.storeOptions.byKey)
				throw new Error("Missing required dataSourceOptions.storeOptions.byKey, byKey must be declared when you use loadMode = 'processed'");

			const innerByKeyFunction = dataSourceOptions.storeOptions.byKey;
			dataSourceOptions.storeOptions.byKey = async function (key) {
				const loadedItem = await innerByKeyFunction(key);
				await preloadColumnsDataAsync(dataSourceOptions, [loadedItem]);

				return loadedItem;
			}
		}

		dataSourceOptions.store = new CustomStore(dataSourceOptions.storeOptions);
		return new DataSource(dataSourceOptions);
	}
}

/**
 * @param {DataGridDataSourceOptions} dataSourceOptions
 * @returns {Promise}
 */
async function preloadColumnsDataAsync(dataSourceOptions, loadedItems) {

	const gridInstance = dataSourceOptions.getDataGrid();

	let preloadPromises = [];

	for (var columnIndex = 0; columnIndex < gridInstance.instance.columnCount(); columnIndex++) {
		const column = gridInstance.instance.columnOption(columnIndex);
		const field = column.dataField;
		const currentLookup = column.lookup;
		const loadByKeys = currentLookup?.dataSource?.store?.byKeys;

		if (!loadByKeys)
			continue;

		const identifiersToPreload = loadedItems.map(item => column.calculateCellValue ? column.calculateCellValue(item) : item[field]);

		preloadPromises.push(loadByKeys(identifiersToPreload).then(result => {

			//HACK: DevExteme DataDrid doesn't support async loading of items to be displayed
			//The following code needs to be used with a grid that contains a custom data source that returns an empty array for the DataGrid first load
			//Search: 18664f34-6fcc-4d8b-8daf-207b9338cbcc
			currentLookup.items = result;
			currentLookup.updateValueMap();
		}));
	}
	await Promise.all(preloadPromises);
}

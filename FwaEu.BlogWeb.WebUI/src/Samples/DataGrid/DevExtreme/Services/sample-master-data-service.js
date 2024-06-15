import MasterDataService from "@/Fwamework/MasterData/Services/master-data-service";
import DataSourceOptionsFactory from "@UILibrary/Modules/MasterData/Services/data-source-options-factory";

const masterDataService = new MasterDataService('SampleEntity', ['id'], false);

// All theses functions are dummified for the sample's sake.
// Do not use this code in your code.

function buildSampleData() {
	const result = [];
	for (let i = 1; i < 100; ++i) {
		result.push({
			id: i,
			name: `Postal code ${i}`
		});
	}
	return result;
}

const sampleData = buildSampleData();

masterDataService.configureAsync = async () => { };
masterDataService.clearCacheAsync = async () => { };

masterDataService.getAllAsync = async function (o) {
	return sampleData;
};

masterDataService.getAllRemoteAsync = async function (options) {
	const start = options.pagination?.skip ?? 0;
	const end = options.pagination?.take != null ? start + options.pagination?.take : sampleData.length;
	return sampleData.slice(start, end);
};

masterDataService.getAsync = async function (id) {
	const result = sampleData.find(item => item.id == id);
	console.log(`getAsync called with id: ${id}`);
	return result;
};

masterDataService.getByIdsAsync = async function (ids) {
	const result = sampleData.filter(item => ids.includes(item.id));
	console.log(`getByIdsAsync called with ids: ${ids}`);
	return result;
};

export default masterDataService;

export const SampleEntityDataSourceOptions = DataSourceOptionsFactory.create(masterDataService);
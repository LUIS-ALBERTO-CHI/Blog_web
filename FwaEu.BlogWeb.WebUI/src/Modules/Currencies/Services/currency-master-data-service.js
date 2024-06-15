import MasterDataService from "@/Fwamework/MasterData/Services/master-data-service";
import DataSourceOptionsFactory from "@UILibrary/Modules/MasterData/Services/data-source-options-factory";

const masterDataService = new MasterDataService('Currencies', ['currencyCode']);
masterDataService.createItem = function(item){
	return {
		...item,
		toString() {
			return `${this.name} (${this.currencyCode})`;
		}
	}
}
export default masterDataService;

export const CurrenciesDataSourceOptions = DataSourceOptionsFactory.create(masterDataService, {
	sort: [{ selector: "name", desc: false }]
});

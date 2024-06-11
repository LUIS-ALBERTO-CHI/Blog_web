import MasterDataService from "@/Fwamework/MasterData/Services/master-data-service";
import DataSourceOptionsFactory from "@UILibrary/Modules/MasterData/Services/data-source-options-factory";

const masterDataService = new MasterDataService('PostalCodes', ['id'], false);
masterDataService.createItem = function (postalCode) {
	return {
		...postalCode,
		toString() {
			return `${postalCode.postalCode} - ${postalCode.townName}`
		}
	};
}
export default masterDataService;

export const FarmPostalCodeDataSourceOptions = DataSourceOptionsFactory.create(masterDataService, {
	sort: [{ selector: "postalCode", desc: false }]
});
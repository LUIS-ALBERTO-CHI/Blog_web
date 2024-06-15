import MasterDataService from "@/Fwamework/MasterData/Services/master-data-service";
import DataSourceOptionsFactory from "@UILibrary/Modules/MasterData/Services/data-source-options-factory";

class ReportFieldMasterDataService extends MasterDataService {
	constructor() {
		super('ReportFields', ['invariantId']);
	}
}

const masterDataService = new ReportFieldMasterDataService();

export default masterDataService;
export const ReportFieldsDataSourceOptions = DataSourceOptionsFactory.create(masterDataService);
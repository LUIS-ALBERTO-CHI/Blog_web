import MasterDataService from "@/Fwamework/MasterData/Services/master-data-service";

class ReportFilterMasterDataService extends MasterDataService {
	constructor() {
		super('ReportFilters', ['invariantId']);
	}
}
export default new ReportFilterMasterDataService();
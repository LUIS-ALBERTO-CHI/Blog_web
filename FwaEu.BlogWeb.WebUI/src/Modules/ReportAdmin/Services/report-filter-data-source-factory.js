import DefaultMasterDataService from "@/Fwamework/MasterData/Services/default-data-loader-service";

export default {

	createSelectBoxDataSource() {
		return {
			key: "invariantId",
			loadMode: "raw",
			load: async () => await DefaultMasterDataService.getMasterDataAsync([{ masterDataKey: "ReportFilters" }]).then(response => {
				response[0].values.forEach(f => f.name = `:${f.invariantId}`);
				return response[0].values;
			})
		}
	}
}
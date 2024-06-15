import { I18n } from "@/Fwamework/Culture/Services/localization-service"
import ReportServerDataSourceHandlerBase from "@/Modules/ReportServerData/Services/report-server-data-source-handler-base";
import { defineAsyncComponent } from "vue";
import FarmActivitiesMasterDataService from '../../Services/farm-activities-master-data-service';

class FarmInfosDataSourceHandler extends ReportServerDataSourceHandlerBase {
	type = "FarmInfos";
	displayOrder = 30;
	icon = "fas fa-house";
	usePreFilters = false;
	useCustomParameters = false;
	createComponent = () => defineAsyncComponent(() => import("../Components/FarmInfosComponent.vue"));
	isRequired = true;
	getDescription() {
		return {
			label: I18n.t("principalActivity")
		};
	}
	async preFetchDataAsync(dataSource) {
		return {
			farmCategories: await FarmActivitiesMasterDataService.getAllAsync()
		};
	}
}	

export default FarmInfosDataSourceHandler;

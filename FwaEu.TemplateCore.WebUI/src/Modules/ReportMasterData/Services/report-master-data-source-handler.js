import MasterDataManagerService from '@/Fwamework/MasterData/Services/master-data-manager-service';
import { I18n } from "@/Fwamework/Culture/Services/localization-service"
import { defineAsyncComponent } from 'vue';

export default {
	type: "MasterData",
	displayOrder: 20,
	icon: "fas fa-list",
	usePreFilters: false,
	useCustomParameters: false,
	createComponent: () => defineAsyncComponent(() => import("@UILibrary/Modules/ReportMasterData/Components/MasterDataComponent.vue")),
	isRequired: true,
	getDescriptionKey() {
		return {
			label: I18n.t("masterData")
		};
	},
	async getDataSourceAsync(invariantId, dataSource, filters, viewContext) {
		return await MasterDataManagerService.getMasterDataAsync(dataSource.argument);
	},
}
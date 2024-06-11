import { I18n } from "@/Fwamework/Culture/Services/localization-service"
import MasterDataManagerService from '@/Fwamework/MasterData/Services/master-data-manager-service';
import { defineAsyncComponent } from "vue";

export default {
	type: "FarmDatas",
	displayOrder: 40,
	icon: "fas fa-house-day",
	usePreFilters: false,
	useCustomParameters: false,
	createComponent: () => defineAsyncComponent(() => import("../Components/FarmDatasComponent.vue")),
	getDescription() {
		return {
			label: I18n.t("chooseFarmDatas")
		};
	},
	setDefaultDataSourceArgument(dataSource) {
		dataSource.argument = '{ "getAnimalSpecies": true, "getActivities": false }';
	},
	async getDataSourceAsync(invariantId, dataSource, filters, viewContext) {
		const parameters = JSON.parse(dataSource.argument);
		let rows = [];
		if (parameters.getAnimalSpecies) {
			const animalSpecies = await MasterDataManagerService.getMasterDataAsync("FarmAnimalSpecies");
			animalSpecies.forEach(function (data) {
				rows.push({ id: data.id, code: data.invariantId, name: data.name });
			});
		}
		if (parameters.getActivities) {
			const farmActivities = await MasterDataManagerService.getMasterDataAsync("FarmActivities");
			farmActivities.forEach(function (data) {
				rows.push({ id: data.id, code: data.invariantId, name: data.name });
			});
		}

		return rows;
	}
}


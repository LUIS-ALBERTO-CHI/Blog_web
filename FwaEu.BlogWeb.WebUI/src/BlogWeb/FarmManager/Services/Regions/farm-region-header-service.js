import { HeaderItem } from "@/Modules/Header/Services/header-item";
import HeaderService from "@/Modules/Header/Services/header-service";
import RouterService from "@/Fwamework/Routing/Services/vue-router-service";
import FarmManagerRoutes from "@/BlogWeb/FarmManager/routes";
import FarmRegionHeaderItemConfiguration from "./farm-region-header-configuration";

export default {
	async configureAsync() {
		const farmManagerRouteNames = FarmManagerRoutes.map(r => r.name);
		const isHeaderVisible = farmManagerRouteNames.includes(RouterService.currentRoute.value?.name);
		HeaderService.register(new HeaderItem(FarmRegionHeaderItemConfiguration, isHeaderVisible));

		RouterService.beforeResolve(async (to,from,next)=>{
			const isHeaderVisible = farmManagerRouteNames.includes(to.name);
			if (isHeaderVisible) {
				const farmRegionHeaderItem = HeaderService.getAllItems().value.find((x) => x.configuration.key === 'FarmRegionHeaderItem');
				if (!farmRegionHeaderItem.fetchedData) {
					farmRegionHeaderItem.fetchedData = await FarmRegionHeaderItemConfiguration.fetchDataAsync();
				}
			}
			HeaderService.setVisibility(FarmRegionHeaderItemConfiguration.key, isHeaderVisible);
			next();
		});
	}
}
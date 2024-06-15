import { defineAsyncComponent } from 'vue';

export default {
	key: "FarmRegionHeaderItem",
	component: defineAsyncComponent(() => import('@/BlogWeb/FarmManager/Components/FarmRegionHeaderComponent.vue')),
	smallModeContentComponent: defineAsyncComponent(() => import('@/BlogWeb/FarmManager/Components/FarmRegionHeaderSmallModeContentComponent.vue')),

	async fetchDataAsync() {
		const regionsMasterData = (await import('@/BlogWeb/FarmManager/Services/Regions/farm-regions-master-data-service')).default;
		const data = {
			regions: await regionsMasterData.getAllAsync()
		};
		return data;
	}
}
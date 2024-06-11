import { defineAsyncComponent } from 'vue';

export default {
	key: "FarmRegionHeaderItem",
	component: defineAsyncComponent(() => import('@/TemplateCore/FarmManager/Components/FarmRegionHeaderComponent.vue')),
	smallModeContentComponent: defineAsyncComponent(() => import('@/TemplateCore/FarmManager/Components/FarmRegionHeaderSmallModeContentComponent.vue')),

	async fetchDataAsync() {
		const regionsMasterData = (await import('@/TemplateCore/FarmManager/Services/Regions/farm-regions-master-data-service')).default;
		const data = {
			regions: await regionsMasterData.getAllAsync()
		};
		return data;
	}
}
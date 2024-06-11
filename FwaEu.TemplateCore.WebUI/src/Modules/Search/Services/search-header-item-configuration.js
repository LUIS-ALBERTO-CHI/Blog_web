import { defineAsyncComponent } from 'vue';

export default {
	key: "SearchHeaderItem",
	component: defineAsyncComponent(() => import('@/Modules/Search/Components/SearchHeaderComponent.vue')),
	smallModeContentComponent: defineAsyncComponent(() => import('@/Modules/Search/Components/SearchHeaderSmallModeContentComponent.vue')),

	async fetchDataAsync() {
		const searchService = (await import('@/Modules/Search/Services/search-service')).default;

		const data = {
			availableSearchItems: await searchService.getSearchProvidersAsync()
		};

		return data;
	}
}
<template>
	<div>
		<i v-if="smallModeEnabled" class="fas fa-search search-icon" />
		<search v-else 
				:search-items="availableSearchItems" 
				:is-small-mode="false" />
	</div>
</template>

<script>
	import Search from "@UILibrary/Modules/Search/Components/SearchComponent.vue";

	export default {
		components: {
			Search
		},
		props: {
			smallModeEnabled: Boolean,
			fetchedData: {
				required: true,
				type: Object
			}
		},
		data() {
			return {
				availableSearchItems: this.$props.fetchedData.availableSearchItems
			};
		},
	}
</script>
<style>
	.search-icon {
		font-size: 25px;
		color: var(--secondary-text-color);
	}
</style>
<template>
	<search :search-items="availableSearchItems" 
			:is-small-mode="true" 
			@hide-content="hideContent"/>
</template>

<script>
	import Search from "@UILibrary/Modules/Search/Components/SearchComponent.vue";

	export default {
		components: {
			Search
		},
		props: {
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
		methods: {
			hideContent() {
				this.$emit('hide-content');
			}
		}
	}
</script>
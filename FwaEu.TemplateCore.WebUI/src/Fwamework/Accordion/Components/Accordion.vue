<template>
	<div class="accordion">
		<!--NOTE: If you want to override the default 'items' slot implementation you will need to bind :accordion-id prop-->
		<slot :accordion-id="id" :items="items" name="items">
			<accordion-item v-for="item in items" :accordion-id="id" v-model:opened="item.opened">
			</accordion-item>
		</slot>
	</div>
</template>

<script>
	import { computed, provide, ref } from "vue";
	import AccordionItem from '@/Fwamework/Accordion/Components/AccordionItem.vue';

	export default {
		components: {
			AccordionItem
		},
		setup() {
			const id = Symbol();
			const openedIndexes = ref([]);
			const count = ref(0);
			provide(id, computed(() => ({
				count,
				openedIndexes
			})));
			return {
				id,
				count,
				openedIndexes
			};
		},
		props: {
			items: Array
		}
	};
</script>

<style lang="scss" scoped>
	.accordion {
		list-style: none;
		margin: 0;
		padding: 0;

		&__item:last-child {
			border-bottom: none;
		}
	}
</style>

<template>
	<dx-select-box :data-source="reportViewsDataSource"
				   :grouped="true"
				   display-expr="name"
				   value-expr="index"
				   v-model:value="selectedValue"
				   @selection-changed="onSelectionChanged"
				   width="250">
		<template #group="{ data }">
			<item :icon="data.displayTypeProperties.icon" :text="data.displayTypeProperties.label" />
		</template>
	</dx-select-box>
</template>
<script>

	import Item from '@UILibrary/Extensions/Components/ItemWithIconTemplateComponent.vue';
	import { DxSelectBox } from 'devextreme-vue/select-box';

	export default {
		components: {
			DxSelectBox,
			Item
		},
		props: {
			reportViews: {
				type: Array,
				required: true
			},
			selectedView: {
				type: Object,
				default: () => { }
			},
		},
		data() {
			return {
				selectedValue: null
			}
		},
		created() {
			this.selectedValue = this.selectedView?.index;
		},
		methods: {
			onSelectionChanged(e) {
				this.reportDisplayType = e.selectedItem.displayType;
				this.$emit('update:selected-view', e.selectedItem);
			}
		},
		computed: {
			reportViewsDataSource() {
				let model = {
					store: this.reportViews,
					key: 'index',
					group: 'displayType.type',
					postProcess: function (groupedData) {
						for (var data of groupedData) {
							data.displayTypeProperties = data.items[0].displayTypeProperties;
						}
						return groupedData;
					}
				};
				return model;
			}
		}
	}
</script>
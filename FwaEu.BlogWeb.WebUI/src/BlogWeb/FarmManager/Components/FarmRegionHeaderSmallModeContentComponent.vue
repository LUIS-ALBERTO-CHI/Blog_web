<template>
		<dx-select-box :data-source="farmRegionsDataSource" 
					   display-expr="name"
					   value-expr="id"
					   v-model="selectedFarmRegionId" 
					   @value-changed="onSelectedRegionChanged">

		</dx-select-box>
</template>

<script>
	import DxSelectBox from 'devextreme-vue/select-box';
	import ViewContextService, { ViewContextModel } from '@/BlogWeb/ViewContext/Services/view-context-service';

	export default {
		components: {
			DxSelectBox
		},
		props: {
			fetchedData: {
				required: true,
				type: Object
			}
		},
		data() {
			const $this = this;
			return {
				farmRegionsDataSource: this.fetchedData.regions,
				selectedFarmRegionId: ViewContextService.get()?.regionId,
				viewContextChangeOff: ViewContextService.onChanged((viewContext) => {
					$this.selectedFarmRegionId = viewContext.regionId;
				})
			};
		},
		unmounted() {
			this.viewContextChangeOff();
		},
		methods: {
			onSelectedRegionChanged() {
				ViewContextService.set(new ViewContextModel(this.selectedFarmRegionId));
				this.$emit('hide-content');
			}
		}
	}
</script>


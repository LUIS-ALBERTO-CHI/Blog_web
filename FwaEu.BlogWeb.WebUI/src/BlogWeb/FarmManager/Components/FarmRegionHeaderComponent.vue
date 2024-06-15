<template>
	<div class="">
		<div class="user-info" v-if="smallModeEnabled">
			<div class="image-container">
				<avatar :item="regionItem" size="medium" />
			</div>
		</div>
		<dx-select-box v-else
					   ref="farmRegionsSelectBox"
					   :data-source="farmRegionsDataSource"
					   display-expr="name"
					   value-expr="id"
					   :value="selectedFarmRegionId"
					   @value-changed="onSelectedRegionChanged">
		</dx-select-box>
	</div>
</template>

<script>
	import DxSelectBox from 'devextreme-vue/select-box';
	import Avatar from '@/Fwamework/Avatar/Components/AvatarComponent.vue';
	import ViewContextService, { ViewContextModel } from '@/BlogWeb/ViewContext/Services/view-context-service';

	export default {
		components: {
			Avatar,
			DxSelectBox
		},
		props: {
			smallModeEnabled: Boolean,
			fetchedData: {
				required: true,
				type: Object
			}
		},
		data() {
			const $this = this;
			return {
				farmRegionsDataSource: this.fetchedData?.regions,
				selectedFarmRegionId: ViewContextService.get()?.regionId,
				menuPositionConfig: {
					my: "top center",
					at: "bottom center"
				},
				viewContextChangeOff: ViewContextService.onChanged((viewContext) => {
					$this.selectedFarmRegionId = viewContext.regionId;
					$this.$forceUpdate();
				})
			};
		},
		unmounted() {
			this.viewContextChangeOff();
		},
		methods: {
			onSelectedRegionChanged(e) {
				ViewContextService.set(new ViewContextModel(e.value));
			}
		},
		computed: {
			selectedFarmRegion() {
				return this.farmRegionsDataSource.find(r => r.id === this.selectedFarmRegionId);
			},
			regionItem() {
				return {
					fullName: this.selectedFarmRegion?.name ?? "...",
					initials: this.selectedFarmRegion && (this.selectedFarmRegion.name[0] + this.selectedFarmRegion.name[this.selectedFarmRegion.name.length - 1]) || "...",
					id: this.selectedFarmRegion?.id ?? -1
				};
			}
		}
	}
</script>
<style scoped>
	.header-component input,
	.header-component div {
		color: var(--secondary-text-color);
	}

	.header-component .dx-texteditor.dx-editor-underlined::after {
		border-color: var(--secondary-text-color);
		border-bottom-color: var(--secondary-text-color);
	}
</style>
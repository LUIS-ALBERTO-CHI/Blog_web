<template>
	<div>
		<span :class="{'disabled-grant-full-access': !model.canGrantFullAccess }">
			<dx-check-box v-model="model.data.hasFullAccess"
						  :text="$t('grantFullAccess')"
						  @value-changed="setFullAccess($event)"
						  :disabled="!model.canGrantFullAccess" />
			<dx-tooltip target=".disabled-grant-full-access"
						width="250px"
						class="user-perimeter-tooltip"
						v-if="!model.readOnly && !model.canGrantFullAccess">
				{{ $t("cannotGrantAccessMessage") }}
			</dx-tooltip>
		</span>
		<div class="block">
			<dx-tree-list :data-source="masterDataModel"
						  :disabled="model.readOnly"
						  v-model:selected-row-keys="selectedRowKeys"
						  key-expr="id"
						  @selection-changed="onSelectionChanged">
				<dx-selection mode="multiple" />
				<dx-column data-field="name" :caption="$t('name')" />
			</dx-tree-list>
		</div>
	</div>

</template>
<script>
	import { DxCheckBox } from 'devextreme-vue/check-box';
	import { DxTooltip } from 'devextreme-vue/tooltip';
	import { DxTreeList, DxSelection, DxColumn } from 'devextreme-vue/tree-list';
	
	export default {
		components: {
			DxCheckBox,
			DxTooltip,
			DxTreeList, DxSelection, DxColumn
		},
		props: {
			fetchedData: {
				type: Array,
				required: true
			},
			modelValue: {
				type: Object,
				required: true,
			}
		},
		data() {
			return {
				model: this.modelValue,
				masterDataModel: [],
				selectedRowKeys: []
			};
		},
		created: function () {
			this.initializePerimeterPart();
		},
		methods: {
			onSelectionChanged(e) {
				this.model.data.accessibleIds = e.selectedRowKeys;
			},
			initializePerimeterPart() {
				let $this = this;

				this.masterDataModel = this.fetchedData.map(p => {
					return {
						id: p.id ?? p.invariantId, name: p.name, selected: $this.model.data.hasFullAccess || $this.model.data.accessibleIds.indexOf(p.id ?? p.invariantId) !== -1
					};
				});
				this.selectedRowKeys = this.masterDataModel.filter(p => p.selected).map(x => x.id);
			},
			setFullAccess(event) {
				this.masterDataModel.forEach(x => x.selected = event.value);
				this.model.data.accessibleIds = this.masterDataModel.filter(p => p.selected).map(x => x.id);
				this.selectedRowKeys = this.masterDataModel.filter(p => p.selected).map(x => x.id);
			}
		},
	}
</script>
<style>
	.user-perimeter-tooltip.dx-popover-wrapper .dx-popup-content {
		white-space: normal !important;
	}
</style>
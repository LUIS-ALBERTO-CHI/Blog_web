<template>
	<div>
		<dx-tree-list :data-source="masterDataModel"
					:disabled="model.readOnly"
					:show-row-lines="true"
					:show-borders="true"
					:column-auto-width="true"
					v-model:selected-row-keys="selectedRowKeys"
					key-expr="invariantId"
					@selection-changed="onSelectionChanged">
			<dx-selection mode="multiple" />
			<dx-column data-field="name" :caption="$t('name')" />
		</dx-tree-list>
	</div>

</template>
<script>
	import { DxTreeList, DxSelection, DxColumn } from 'devextreme-vue/tree-list';
	
	export default {
		components: {
			DxTreeList,
			DxSelection,
			DxColumn
		},
		props: {
			fetchedData: {
				type: Array,
				required: true
			},
			modelValue: {
				type: Object,
				required: true
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
			this.initializePermissionPart();
		},
		methods: {
			initializePermissionPart() {
				let $this = this;
				this.masterDataModel= this.fetchedData.map(p => {
					return { invariantId: p.invariantId, name: p.name, selected: $this.model.data.selectedIds.indexOf(p.invariantId) !== -1 };
				});
				$this.selectedRowKeys = $this.masterDataModel.filter(p => p.selected).map(x => x.invariantId);
			},
			onSelectionChanged(e) {
				this.model.data.selectedIds = e.selectedRowKeys;
			},
		},
	}
</script>
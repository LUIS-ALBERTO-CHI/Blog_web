<template>
	<div>
		<dx-tree-list :data-source="masterDataModel"
					:disabled="model.readOnly"
					:show-row-lines="true"
					:show-borders="true"
					:column-auto-width="true"
					v-model:selected-row-keys="selectedRowKeys"
					key-expr="id"
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
			this.initializeRolePart();
		},
		methods: {
			initializeRolePart() {
				let $this = this;
				this.masterDataModel = this.fetchedData.map(r => {
					return { name: r.name, id: r.id, selected: $this.model.data.selectedIds.indexOf(r.id) !== -1 };
				});
				$this.selectedRowKeys = $this.masterDataModel.filter(r => r.selected).map(x => x.id);
			},
			onSelectionChanged(e) {
				this.model.data.selectedIds = e.selectedRowKeys;
			},
		}
	}
</script>
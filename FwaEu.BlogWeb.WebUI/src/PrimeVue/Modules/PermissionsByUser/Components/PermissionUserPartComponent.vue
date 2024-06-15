<template>
	<div>
		<DataTable :value="masterDataModel"
				   :disabled="model.readOnly"
				   v-model:selection="selectedRows"
				   dataKey="invariantId"
				   showGridlines>
			<Column selectionMode="multiple" headerStyle="width: 3rem"></Column>
			<Column field="name" :header="$t('name')"></Column>
		</DataTable>
	</div>

</template>
<script>
	import DataTable from 'primevue/datatable';
	import Column from 'primevue/column';
	export default {
		components: {
			DataTable,
			Column,
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
				selectedRows: []
			};
		},
		created: function () {
			this.initializePermissionPart();
		},
		methods: {
			initializePermissionPart() {
				let $this = this;
				this.masterDataModel = this.fetchedData.map(p => {
					return { invariantId: p.invariantId, name: p.name, selected: $this.model.data.selectedIds.indexOf(p.invariantId) !== -1 };
				});
				$this.selectedRows = $this.masterDataModel.filter(p => p.selected);
			}
		},
		watch: {
			selectedRows() {
				this.model.data.selectedIds = this.selectedRows.map(x => x.invariantId);
			}
		}
	}
</script>
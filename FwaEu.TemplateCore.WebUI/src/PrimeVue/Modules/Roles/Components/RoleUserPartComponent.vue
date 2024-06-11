<template>
	<div>
		<DataTable :value="masterDataModel"
				   v-model:selection="selectedRowKeys"
				   dataKey="id"
				   :disabled="model.readOnly"
                   showGridlines >
			<Column selectionMode="multiple" headerStyle="width: 3rem"></Column>
			<Column field="name" :header="$t('name')"></Column>
		</DataTable>
	</div>
</template>
<script>
	import DataTable  from 'primevue/datatable';
	import Column from 'primevue/column';

	export default {
		components: {
			DataTable,
			Column
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
		}
	}
</script>
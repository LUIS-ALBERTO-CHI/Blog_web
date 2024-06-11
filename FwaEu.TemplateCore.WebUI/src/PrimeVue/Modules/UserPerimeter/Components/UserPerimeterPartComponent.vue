<template>
	<div>
		<span :class="{'disabled-grant-full-access': !model.canGrantFullAccess }">
			<div>
				<Checkbox v-model="model.data.hasFullAccess"
						  @input="setFullAccess"
						  :disabled="!model.canGrantFullAccess"
						  :binary="true" />
				<label class="ml-2"> {{ $t('grantFullAccess') }} </label>
			</div>
			<span v-if="!model.readOnly && !model.canGrantFullAccess" v-tooltip="$t('cannotGrantAccessMessage')">
			</span>
		</span>
		<div class="block">
			<DataTable :value="masterDataModel"
					   v-model:selection="selectedRows"
					   dataKey="id"
					   :disabled="model.readOnly"
					   showGridlines>
				<Column :hidden="model.data.hasFullAccess" selectionMode="multiple" headerStyle="width: 3rem" />
				<Column field="name" :header="$t('name')" />
			</DataTable>
		</div>
	</div>
</template>
<script>
	import Checkbox from 'primevue/checkbox';
	import DataTable from 'primevue/datatable';
	import Column from 'primevue/column';

	export default {
		components: {
			Checkbox,
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
				required: true,
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
			this.initializePerimeterPart();
		},
		methods: {
			initializePerimeterPart() {
				let $this = this;
				this.masterDataModel = this.fetchedData.map(p => {
					return {
						id: p.id ?? p.invariantId, name: p.name, selected: $this.model.data.hasFullAccess || $this.model.data.accessibleIds.indexOf(p.id ?? p.invariantId) !== -1
					};
				});

				this.selectedRows = this.masterDataModel.filter(p => p.selected);
			},
			setFullAccess(checked) {

				this.masterDataModel.forEach(x => x.selected = checked);
				this.model.data.accessibleIds = this.masterDataModel.filter(p => p.selected).map(x => x.id);
				this.selectedRows = this.masterDataModel.filter(p => p.selected);
			},
		},
		watch: {
			selectedRows() {
				this.model.data.accessibleIds = this.selectedRows.map(x => x.id);
			}
		}
	}
</script>
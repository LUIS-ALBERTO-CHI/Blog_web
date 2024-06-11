<template>
	<PageContainer type="summary">
		<Box>
			<pre>
				Check your javascript console to see only 1 master data request with multiple keys.
			</pre>
			<DxDataGrid :data-source="sampleDataSource"
						width="100%"
						ref="sampleGrid">
				<DxColumn data-field="name" caption="name" :min-width="150" />
				<DxColumn data-field="postalCodeId" caption="postalCode" width="20%" :min-width="150" :lookup="postalCodeSelectBoxOptions" />
			</DxDataGrid>
		</Box>
	</PageContainer>
</template>
<script>
	import SampleDataGridData from "../../Services/sample-data-grid-data";

	import DataGridDataSourceFactory from "@UILibrary/Modules/MasterData/Services/data-grid-data-source-factory";
	import { SampleEntityDataSourceOptions } from "@/Samples/DataGrid/DevExtreme/Services/sample-master-data-service";

	import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue";
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import { DxDataGrid, DxColumn } from "devextreme-vue/data-grid";

	export default {
		components: {
			PageContainer,
			Box,
			DxDataGrid,
			DxColumn
		},
		data() {

			return {
				postalCodeSelectBoxOptions: {
					displayExpr: function (postalCode) {
						return postalCode ? postalCode?.name : "";
					},
					dataSource: SampleEntityDataSourceOptions,
					valueExpr: 'id'
				},
				sampleDataSource: DataGridDataSourceFactory.createDataSource({
					getDataGrid: () => this.$refs.sampleGrid,
					storeOptions: { load: this.loadSampleDataAsync }
				}),
			};
		},
		methods: {
			async loadSampleDataAsync() {
				return await SampleDataGridData.getSampleDataAsync(2500);
			}
		}
	};
</script>
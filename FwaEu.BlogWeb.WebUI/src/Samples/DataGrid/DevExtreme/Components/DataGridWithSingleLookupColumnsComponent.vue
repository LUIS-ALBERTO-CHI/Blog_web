<template>
	<PageContainer type="summary">
		<Box>
			<pre>
				Check your javascript console to see 1 master data request per line.
				<a href="https://supportcenter.devexpress.com/ticket/details/t672141/datagrid-how-to-reduce-the-number-of-loaded-items-in-a-lookup-column#approvedAnswers">The DxColumn's show-editor-always attribute must be true for lookup columns to load async dataSources</a>
			</pre>
			<DxDataGrid :data-source="sampleDataSource"
						width="100%"
						ref="sampleGrid">
				<DxColumn data-field="name" caption="name" :min-width="150" />
				<DxColumn data-field="postalCodeId" caption="postalCode" width="20%" :min-width="150" :show-editor-always="true" :lookup="postalCodeSelectBoxOptions" />
			</DxDataGrid>
		</Box>
	</PageContainer>
</template>
<script>
	import SampleDataGridData from "../../Services/sample-data-grid-data";

	import { SampleEntityDataSourceOptions } from "@/Samples/DataGrid/DevExtreme/Services/sample-master-data-service";

	import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue";
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import { DxDataGrid, DxColumn } from "devextreme-vue/data-grid";
	import DataSource from "devextreme/data/data_source";

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
				sampleDataSource: new DataSource({
					load: SampleDataGridData.getSampleDataAsync
				})
			};
		},
		methods: {
			async loadSampleDataAsync() {
				return await SampleDataGridData.getSampleDataAsync(2500);
			}
		}
	};
</script>
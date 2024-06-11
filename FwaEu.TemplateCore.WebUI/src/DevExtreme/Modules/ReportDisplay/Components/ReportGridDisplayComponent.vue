<template>
	<dx-data-grid v-if="columns"
				  ref="dataGrid"
				  :data-source="dataSource"
				  :columns="columns"
				  :allow-column-resizing="true"
				  :column-auto-width="true"
				  :allow-column-reordering="true"
				  @initialized="onDataGridInitialized"
				  @content-ready="onContentReady"
				  @exporting="(e) => onExporting(e, exportFileName)"
				  :loadPanel="{onShowing: null, onHiding: null, delay: 0, width: 350}">
		<dx-search-panel :visible="true" :width="250" />
		<dx-export :enabled="true" />
		<dx-header-filter :visible="true" />
		<dx-paging :page-size="25" />
		<dx-pager :show-page-size-selector="true"
				  :allowed-page-sizes="pageSizes"
				  :show-info="true" :visible="true" />
		<dx-filter-row :visible="true" />
		<dx-column-chooser :enabled="true" />
	</dx-data-grid>
</template>
<script>
	import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";
	import { DxDataGrid, DxPaging, DxPager, DxFilterRow, DxHeaderFilter, DxExport, DxSearchPanel, DxColumnChooser } from 'devextreme-vue/data-grid';
	import ReportsFieldsServiceService from '@/Modules/ReportDisplay/Services/reports-fields-service';
	import { onExporting } from '@/DevExtreme/Extensions/Services/export-helper';

	export default {
		components: {
			DxDataGrid,
			DxPaging,
			DxPager,
			DxFilterRow,
			DxHeaderFilter,
			DxExport,
			DxSearchPanel,
			DxColumnChooser
		},
		props: {
			report: {
				type: Object
			},
			dataSource: {
				type: Array,
				required: true
			},
			asyncLoadingMessage: String,
			asyncDataSourceLoading: Boolean,
			showLoadingPanelOnCreated: Boolean,
		},
		data() {
			return {
				pageSizes: [10, 25, 50, 100],
				columns: null,
				exportFileName: this.report.name,
				currentView: null,
			}
		},
		watch: {
			currentView(newVal) {
				if (this.$refs.dataGrid) {
					this.$refs.dataGrid.instance.state(newVal);
				}
			}
		},
		created: async function () {
			if (this.showLoadingPanelOnCreated)
				await this.loadComponentWitLoadingPanelAsync();
			else
				await this.loadComponentWithoutLoadingPanelAsync();
		},
		methods: {
			loadComponentWitLoadingPanelAsync: showLoadingPanel(async function () {
				await this.loadComponentAsync();
			}),
			async loadComponentWithoutLoadingPanelAsync() {
				await this.loadComponentAsync();
			},
			async loadComponentAsync() {
				let properties = await ReportsFieldsServiceService.getPropertiesAsync(this.report);
				this.columns = properties.map(p => ReportsFieldsServiceService.createFieldsGrid(p, null, this.$i18n));
			},
			getCurrentView() {
				return JSON.stringify(this.$refs.dataGrid.instance.state());
			},
			setCurrentView(viewValue) {
				this.currentView = viewValue ? JSON.parse(viewValue) : null;
			},
			onDataGridInitialized() {
				if (this.currentView) {
					this.$refs.dataGrid.instance.state(this.currentView);
				}
			},
			onContentReady(e) {
				if (this.asyncDataSourceLoading) {
					this.$refs.dataGrid.instance.beginCustomLoading(this.asyncLoadingMessage);
				}
			},
			onExporting
		}
	}
</script>
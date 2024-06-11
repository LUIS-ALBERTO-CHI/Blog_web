<template>
	<div>
		<dx-pivot-grid v-if="pivotDataSource"
					   ref="pivotGrid"
					   :data-source="pivotDataSource"
					   :allow-sorting-by-summary="true"
					   :allow-filtering="true"
					   :height="height"
					   :allow-expand-all="true"
					   @initialized="onPivotGridInitialized">
			<dx-field-chooser :enabled="true"
							  apply-changes-mode="onDemand" />
			<dx-field-panel :show-column-fields="true"
							:show-data-fields="true"
							:show-filter-fields="true"
							:show-row-fields="true"
							:allow-field-dragging="true"
							:visible="true" />
			<dx-scrolling mode="virtual" />
			<dx-load-panel :visible="loadingVisible"
						   :message="asyncLoadingMessage"
						   width="350">
			</dx-load-panel>
		</dx-pivot-grid>
	</div>
</template>
<script>
	import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";
	import { DxPivotGrid, DxFieldChooser, DxFieldPanel, DxScrolling } from 'devextreme-vue/pivot-grid';
	import ReportsFieldsServiceService from '@/Modules/ReportDisplay/Services/reports-fields-service';
	import { DxLoadPanel } from 'devextreme-vue/load-panel';

	export default {
		components: {
			DxPivotGrid,
			DxFieldChooser,
			DxFieldPanel,
			DxScrolling,
			DxLoadPanel
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
			height: Number,
			showLoadingPanelOnCreated: Boolean,
		},
		data() {
			return {
				pivotDataSource: null,
				exportFileName: this.report.name,
				currentView: null,
				loadingVisible: this.asyncDataSourceLoading,
			}
		},
		watch: {
			currentView(newValue) {
				if (this.$refs.pivotGrid)
					this.$refs.pivotGrid.instance.getDataSource().state(newValue);
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
				let customFields = properties.map(p => ReportsFieldsServiceService.createFieldsPivot(p, null, this.$i18n));
				let dataSource = {
					store: this.dataSource,
					fields: customFields
				};
				this.pivotDataSource = dataSource;
			},
			getCurrentView() {
				return JSON.stringify(this.$refs.pivotGrid.instance.getDataSource().state());
			},
			setCurrentView(viewValue) {
				this.currentView = viewValue ? JSON.parse(viewValue) : null;
			},
			onPivotGridInitialized() {
				if (this.currentView) {
					this.$refs.pivotGrid.instance.getDataSource().state(this.currentView);
				}
			}
		}
	}
</script>
<template>
	<page-container type="list">
		<box :menu-items="menuItems" :menu-options="{forceMenuMode: true}">
			<report-view-select v-if="reportViews"
								:report-views="reportViews"
								v-model:selected-view="selectedView" />

			<div v-if="selectedView">
				<component ref="reportDisplayTypeComponent"
						   :is="selectedView.displayType.createComponent()"
						   :data-source="dataSource"
						   v-mounted="onReportDisplayComponentMounted"
						   :report="currentReport"
						   :height="1000"
						   :async-loading-message="$t('messageReportAsyncLoading')"
						   :async-data-source-loading="asyncDataSourceLoading"
						   :show-loading-panel-on-created="true" />
			</div>
		</box>



	</page-container>
</template>
<script>
	import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue";
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import ReportViewSelect from "@UILibrary/Modules/Reports/Components/ReportViewSelectComponent.vue";
	import ReportDisplayService from "@/Modules/ReportDisplay/Services/report-display-service";
	import ReportsService from "@/Modules/Reports/Services/reports-service";
	import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";
	import ReportDataSourceService from "@/Modules/Reports/Services/report-data-source-service";
	import Toolbar from "@/Fwamework/Toolbar/Components/ToolbarComponent.vue";
	import { AsyncLazy } from '@/Fwamework/Core/Services/lazy-load';
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
	import ReportAsyncDataSourceService from "@/Modules/Reports/Services/report-async-data-source-service";
	import NotificationService from "@/Fwamework/Notifications/Services/notification-service";


	export default {
		components: {
			PageContainer,
			Box,
			Toolbar,
			ReportViewSelect
		},
		props: {
			filterValues: {
				type: Object
			},
			invariantId: {
				type: String,
				required: true
			}
		},
		data() {
			let $this = this;
			return {
				selectedView: null,
				reportViews: null,
				dataSource: null,
				asyncDataSourceLoading: false,
				currentReportLazy: new AsyncLazy(() => ReportsService.getByInvariantIdAsync($this.invariantId)),
				menuItems: [],
				messageReportAsyncLoading: ""
			}
		},
		created: showLoadingPanel(async function () {
			await loadMessagesAsync(this, import.meta.glob('@/Modules/Reports/Components/Content/report-page-messages.*.json'));

			let currentReport = await this.currentReportLazy.getValueAsync();
			currentReport.invariantId = this.invariantId;
			let viewContext = {};//TODO: Gérer viewContext
			let filters = this.filterValues ?? {};
			let data = null;
			if (currentReport.isAsync) {
				if (this.$route.query.reportCacheStoreKey) {
					await ReportAsyncDataSourceService.getDataReportAsync(this.$route.query.reportCacheStoreKey).then(result => {
						data = result;
					}).catch(error => {
						if (error.response.status === 404) {
							NotificationService.showWarning(this.$t('cacheKeyNotFound'));
						} else {
							throw error;
						}
					});
				}
				else {
					await ReportAsyncDataSourceService.queueDataSourceReportAsync(currentReport, filters);
					this.asyncDataSourceLoading = true;
					data = [];
				}
			}
			else {
				data = await ReportDataSourceService.getDataSourceAsync(currentReport.invariantId, currentReport.dataSource, filters, viewContext);
			}

			let index = 0;

			if (currentReport.defaultViews) {
				let viewReportData = Object.keys(currentReport.defaultViews).map(displayTypeName => {
					let displayType = ReportDisplayService.get(displayTypeName);

					return currentReport.defaultViews[displayTypeName].map(item => {
						return {
							index: index++,
							value: item.value,
							name: item.name,
							isDefault: item.isDefault,
							displayType: displayType,
							displayTypeProperties: displayType.getDescriptionKey()
						};
					});
				}).flat();

				if (viewReportData && viewReportData.length > 0) {
					this.reportViews = viewReportData;
					this.selectedView = viewReportData.find(v => v.isDefault);
				}
				else {
					this.selectedView = {
						displayType: ReportDisplayService.getFirstDefaultProvider(),
					};
				}
			}

			this.currentReport = currentReport;
			this.dataSource = data;

			let $this = this;
			if (this.filterValues) {
				this.menuItems = [{
					text: $this.$t('modifyFilters'),
					icon: "edit",
					action() {
						$this.$router.push({
							name: "ReportFilter",
							params: { invariantId: $this.invariantId },
							query: { filters: JSON.stringify(filters) }
						});
					}
				}]
			}
		}),

		methods: {
			onReportDisplayComponentMounted() {
				if (this.selectedView) {
					this.$refs.reportDisplayTypeComponent.setCurrentView(this.selectedView.value);
				}
			},

			async onNodeResolve(node) {
				let invariantId = this.$route.params.invariantId;
				if (invariantId) {
					let report = await this.currentReportLazy.getValueAsync();
					let name = report.name;
					node.text = name;
				}
				return node;
			},
			getMessageReportAsyncLoading() {
				return this.$t('messageReportAsyncLoading');
			}

		},
		watch: {
			selectedView(newVal, oldVal) {
				if (oldVal
					&& newVal.displayType === oldVal.displayType
					&& this.$refs.reportDisplayTypeComponent) {

					this.$refs.reportDisplayTypeComponent.setCurrentView(newVal.value);
				}
			}
		}
	}
</script>
<style>
	.messageReportAsyncLoading {
		margin-top: 30px;
		margin-bottom: -20px;
	}
</style>
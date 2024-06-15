<template>
	<page-container type="form">
		<box v-if="filters">
			<report-filter :filters="filters"
						   :filter-values="filterValues"
						   ref="reportFilters" />
			<div class="form-buttons">
				<dx-button :text="$t('apply')"
						   @click="applyAsync"
						   type="success" />
			</div>
		</box>
	</page-container>
</template>

<script>
	import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue";
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import ReportFilter from '@UILibrary/Modules/Reports/Components/ReportFilterComponent.vue';
	import DxButton from 'devextreme-vue/button';
	import { showLoadingPanel } from '@/Fwamework/LoadingPanel/Services/loading-panel-service';
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
	import ReportService from '@/Modules/Reports/Services/reports-service';
	import ReportFilterMasterDataService from "@/Modules/ReportMasterData/Services/report-filter-master-data-service";
	import { AsyncLazy } from '@/Fwamework/Core/Services/lazy-load';


	export default {

		components: {
			PageContainer,
			Box,
			ReportFilter,
			DxButton
		},
		data() {
			let $this = this;
			return {
				filters: null,
				currentReportLazy: new AsyncLazy(() => ReportService.getByInvariantIdAsync($this.invariantId))

			}
		},
		props: {
			invariantId: String,
			filterValues: {
				type: Object
			},

		},
		created: showLoadingPanel(async function () {
			await loadMessagesAsync(this, import.meta.glob('@/Modules/Reports/Components/Content/report-filter-messages.*.json'));

			let report = await this.currentReportLazy.getValueAsync();
			this.filters = await Promise.all(
				report.filters
					.map(filter => ReportFilterMasterDataService.getAsync(filter.invariantId)
						.then(f => {
							f.dataSource = { type: f.dataSourceType, argument: f.dataSourceArgument },
								f.isRequired = filter.isRequired;
							return f;
						})));
		}),
		methods: {
			async applyAsync() {
				let isValid = await this.$refs.reportFilters.validateAsync();
				if (isValid) {
					let filters = this.$refs.reportFilters.getReportFilters();
					this.$router.push({
						name: "Report",
						params: { invariantId: this.invariantId },
						query: { filters: JSON.stringify(filters) }
					});
				}
			},
			async onNodeResolve(node) {
				let invariantId = this.$route.params.invariantId;
				if (invariantId) {
					const report = await this.currentReportLazy.getValueAsync();
					node.text = this.$t('prefilterReport', { report: report.name });
				}
				return node;
			}
		}
	}
</script>

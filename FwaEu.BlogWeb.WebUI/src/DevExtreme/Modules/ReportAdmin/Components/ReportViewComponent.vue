<template>
	<div>
		<localizable-string-input validation-type="all"
								  v-model="tab.view.name"
								  ref="viewName" />
		<dx-check-box v-model="tab.view.isDefault"
					  :text="$t('isDefaultView')"></dx-check-box>
		<div v-if="selectedView">
			<component ref="reportDisplayTypeComponent"
					   :is="selectedView.displayType.createComponent()"
					   :data-source="data"
					   v-mounted="onReportDisplayComponentMounted"
					   :report="currentReport"
					   :async-loading-message="$t('messageReportAsyncLoading')" />
		</div>
	</div>
</template>

<script>

	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
	import LocalizableStringInput from "@UILibrary/Fwamework/Culture/Components/LocalizableStringInputComponent.vue";
	import { DxCheckBox } from 'devextreme-vue/check-box';
	import ReportAdminService from '@/Modules/ReportAdmin/Services/report-admin-service';
	import ReportDisplayService from "@/Modules/ReportDisplay/Services/report-display-service";
	import ReportsService from "@/Modules/Reports/Services/reports-service";
	import { AsyncLazy } from '@/Fwamework/Core/Services/lazy-load';

	export default {
		components: {
			LocalizableStringInput,
			DxCheckBox,
		},
		props: {
			dataSource: Object,
			loadParameters: Array,
			report: Object,
			invariantId: String,
		},
		data() {
			return {
				tab: this.dataSource,
				reportModel: this.report,
				reportLazy: new AsyncLazy(() => ReportsService.getByInvariantIdAsync(this.invariantId)),
				data: {},
				currentReport: {},
				reportViews: null,
				selectedView: null,
			};
		},
		created: async function () {
			await loadMessagesAsync(this, import.meta.glob('@/Modules/ReportAdmin/Components/Content/report-view.*.json'));

			let currentReport = await this.reportLazy.getValueAsync();
			currentReport.invariantId = this.invariantId;
			this.data = await ReportAdminService.getReportDataForAdmin(this.reportModel, this.loadParameters);
			let displayType = ReportDisplayService.get(this.tab.viewType);
			this.selectedView =
			{
				value: this.tab.view.value,
				name: this.tab.view.name,
				isDefault: this.tab.view.isDefault,
				displayType: displayType,
				displayTypeProperties: displayType.getDescriptionKey()
			};
			this.currentReport = currentReport;
		},
		methods: {
			onReportDisplayComponentMounted() {
				if (this.selectedView) {
					this.$refs.reportDisplayTypeComponent.setCurrentView(this.tab.view.value);
				}
			},
			refreshReportModelView() {
				this.tab.view.value = this.$refs.reportDisplayTypeComponent ? this.$refs.reportDisplayTypeComponent.getCurrentView() : {};
			},
			isValid() {
				return this.$refs.viewName.validate();
			},
		},
		watch: {
			'dataSource.view.isDefault'(newValue) {
				if (newValue === true)
					this.$emit('tab-is-default-changed');
			},
			'dataSource.view.name': {
				deep: true,
				handler: function (newValue) {
					if (this.tab.isNew) {
						this.tab.isNew = false;
						this.$emit('tab-adding');
					}
				},
			},
		}
	}
</script>

<style scoped>
</style>
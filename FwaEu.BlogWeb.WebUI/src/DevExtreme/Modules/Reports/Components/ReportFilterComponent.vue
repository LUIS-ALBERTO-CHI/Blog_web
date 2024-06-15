<template>
	<div>
		<dx-form :form-data="reportFilters"
				 :read-only="readOnly"
				 ref="reportFiltersForm"
				 :items="formItems">
		</dx-form>

	</div>
</template>
<script>
	import { DxForm } from 'devextreme-vue/form';
	import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";
	import ReportsFilterServiceService from '@/Modules/Reports/Services/reports-filters-service';

	export default {
		components: {
			DxForm
		},
		data() {
			return {
				formItems: [],
				reportFilters: {}
			}
		},
		props: {
			filters: Array,
			filterValues: {
				type: Object
			},
			readOnly: Boolean
		},
		created: showLoadingPanel(async function () {
			this.formItems = await Promise.all(this.filters.map(filter => ReportsFilterServiceService.createFormItem(filter)));
			this.reportFilters = this.filterValues ?? {};
		}),
		methods: {
			getReportFilters() {
				return this.reportFilters;
			},
			validateAsync() {
				let validatorResult = this.$refs.reportFiltersForm.instance.validate();
				return validatorResult.isValid;
			}
		}
	}
</script>

<template>
	<div class="load-data-source-component">
		<dx-data-grid ref="loadDataSourceComponent"
					  :data-source="dataSource"
					  @init-new-row="onInitNewRow"
					  @editor-preparing="onEditorPreparing"
					  @cell-prepared="onCellPrepared"
					  @row-validating="onRowValidating">
			<dx-paging :enable="false" />
			<dx-sorting mode="none" />
			<dx-filter-row :visible="false" />
			<dx-editing :allow-updating="true"
						:allow-adding="allowAdding"
						:allow-deleting="true"
						mode="cell">
			</dx-editing>
			<dx-search-panel :visible="false" />
			<dx-column type="buttons">
			</dx-column>
			<dx-column data-field="invariantId"
					   data-type="string"
					   :caption="$t('name')">
				<dx-required-rule />
			</dx-column>
			<dx-column data-field="dotNetTypeName"
					   data-type="string"
					   :caption="$t('dotNetTypeName')">
				<dx-lookup :data-source="dotNetTypeNameSelectBoxOptions"
						   display-expr="key"
						   value-expr="value" />
				<dx-required-rule />
			</dx-column>
			<dx-column data-field="dataSource"
					   data-type="string"
					   :caption="$t('dataSource')"
					   :allow-editing="false">
			</dx-column>
			<dx-column data-field="value"
					   edit-cell-template="valueCellTemplate"
					   :caption="$t('value')" />
			<template #valueCellTemplate="{ data }">
				<dx-form :form-data="data.data">
					<dx-simple-item :editor-type="getSimpleDxItem(data.data)"
									data-field="value">
						<dx-label :visible="false"></dx-label>
					</dx-simple-item>
				</dx-form>
			</template>
		</dx-data-grid>
	</div>
</template>

<script>
	import { DxSimpleItem, DxForm, DxLabel } from 'devextreme-vue/form';
	import {
		DxDataGrid, DxColumn, DxFilterRow, DxEditing, DxSearchPanel, DxRequiredRule, DxLookup, DxPaging, DxSorting
	} from 'devextreme-vue/data-grid';
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
	import ReportsFilterServiceService from '@/Modules/Reports/Services/reports-filters-service';

	export default {
		components: {
			DxDataGrid,
			DxColumn,
			DxFilterRow,
			DxEditing,
			DxPaging,
			DxSorting,
			DxSearchPanel,
			DxRequiredRule,
			DxSimpleItem,
			DxForm,
			DxLabel,
			DxLookup,
		},
		props: {
			dataSource: {
				type: Array,
				required: true
			},
			allowAdding: Boolean,
		},
		data() {
			return {
				simpleDxItemsCache: [],
				dotNetTypeNameSelectBoxOptions: ReportsFilterServiceService.getParameterTypes(),
			}
		},
		async created() {
			await loadMessagesAsync(this, import.meta.glob('@/Modules/ReportAdmin/Components/Content/load-data-source-messages.*.json'));
		},
		methods: {
			validateAsync() {
				return Promise.resolve(!this.$refs.loadDataSourceComponent.instance.hasEditData());
			},
			getSimpleDxItem(filter) {
				let reportFilterInCache = this.simpleDxItemsCache.find(x => x.dotNetTypeName == filter.dotNetTypeName);
				if (!reportFilterInCache) {
					reportFilterInCache = {
						dotNetTypeName: filter.dotNetTypeName,
						editorType: ReportsFilterServiceService.createFormItem(filter).editorType
					};
					this.simpleDxItemsCache.push(reportFilterInCache);
				}
				return reportFilterInCache.editorType;
			},
			onInitNewRow: function ($event) {
				$event.data.dotNetTypeName = "String"; // Default dotNetType
			},
			onEditorPreparing: function ($event) {
				if ($event.parentType == "dataRow") {
					$event.editorOptions.disabled = $event.row.data.dataSource == 'Filter';
				}
			},
			onCellPrepared: function ($event) {
				if ($event.rowType === "data" && $event.column.command == "edit" && $event.data.dataSource == 'Filter') {
					$event.cellElement.innerHTML = "";
				}
			},
			onRowValidating(e) {
				if (e.newData.invariantId) {
					if (this.dataSource.find(x => x.__KEY__ != e.newData.__KEY__ && x.invariantId == e.newData.invariantId)) {
						e.errorText = this.$t('multipleFilterValidationError');
						e.isValid = false;
					}
				}
			},
		},
	}

</script>

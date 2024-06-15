<template>
	<div>
		<dx-form :col-count="2"
				 validation-group="dataAndFilters"
				 ref="dataAndFiltersForm"
				 :form-data="dataObject">

			<dx-item :is-required="true"
					 :editor-options="dataSourceTypeSelectBoxOptions"
					 data-field="dataSource.type"
					 editor-type="dxSelectBox">
				<dx-label :text="$t('dataSourceType')" />
			</dx-item>
			<dx-empty-item :col-span="1" />
			<dx-empty-item :col-span="2" />

			<template #datagridTemplate>
				<dx-data-grid v-if="showPrefilters"
							  :data-source="dataObject.filters"
							  ref="dataAndFiltersGrid"
							  @init-new-row="onInitNewRow"
							  @row-validating="onRowValidating">
					<dx-toolbar>
						<dx-toolbar-item location="after"
								 template="actionMenu" />
					</dx-toolbar>
					<dx-paging :enable="false" />
					<dx-sorting mode="none" />
					<dx-editing mode="cell"
								:allow-adding="true"
								:allow-updating="true"
								:allow-deleting="true"
								:confirm-delete="true" />
					<dx-column type="buttons" width="140px" />
					<dx-column data-field="invariantId"
							   :caption="$t('filterCode')"
							   :lookup="reportFilterCodesDataSource">
						<DxRequiredRule />
					</dx-column>
					<dx-column data-field="isRequired"
							   data-type="boolean"
							   :caption="$t('isRequired')">
					</dx-column>
					<template #actionMenu>
						<action-menu :items="menus" />
					</template>
				</dx-data-grid>
			</template>

			<dx-item :col-span="2"
					 template="datagridTemplate">
				<dx-label :text="$t('prefilters')" :visible="showPrefilters" />
			</dx-item>
			<dx-empty-item :col-span="2" />
			<template #dataSourceTypeInputTemplate>
				<div v-if="dataSourceTypeControl">
					<component ref="dataSourceTypeInputComponent"
							   :is="dataSourceTypeControl.createComponent()"
							   v-model="dataObject.dataSource.argument"
							   :pre-fetched-data="argumentComponentpreFetchedData">
					</component>
				</div>
			</template>

			<dx-item v-if="dataSourceTypeControl"
					 :col-span="2"
					 :is-required="dataSourceTypeControl.isRequired"
					 template="dataSourceTypeInputTemplate"
					 data-field="dataSource">
				<dx-label :text="datasourceTypeControlLabel" />
			</dx-item>
		</dx-form>
	</div>
</template>

<script>
	import { DxForm, DxItem, DxLabel, DxEmptyItem } from "devextreme-vue/form"
	import { DxDataGrid, DxColumn, DxEditing, DxRequiredRule, DxPaging, DxSorting, DxHeaderFilter, DxToolbar, DxItem as DxToolbarItem } from 'devextreme-vue/data-grid';
	import ActionMenu from "@UILibrary/Fwamework/ActionMenu/Components/ActionMenuComponent.vue";
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
	import ReportFilterDataSourceFactory from "@/Modules/ReportAdmin/Services/report-filter-data-source-factory.js";
	import DataAndFiltersService from "@/Modules/ReportAdmin/Services/data-and-filters-service";
	import ReportDataSourceService from "@/Modules/Reports/Services/report-data-source-service";

	export default {
		components: {
			DxForm,
			DxItem,
			DxLabel,
			DxEmptyItem,
			DxDataGrid,
			DxHeaderFilter,
			DxColumn,
			DxEditing,
			DxPaging,
			DxSorting,
			DxToolbar,
			DxToolbarItem,
			DxRequiredRule,
			ActionMenu
		},
		props: {
			modelValue: {
				type: Object,
				required: true
			}
		},
		data() {
			const $this = this;
			return {
				dataObject: Object.assign({}, this.modelValue),
				dataSourceTypeSelectBoxOptions: {
					valueExpr: "type",
					displayExpr: "type",
					dataSource: ReportDataSourceService.getAllDataSourceTypesOrderedForDropdown(),
					onValueChanged: this.dataSourceTypeValueChanged,
				},
				reportFilterCodesDataSource: {
					valueExpr: "invariantId",
					displayExpr: "name",
					dataSource: ReportFilterDataSourceFactory.createSelectBoxDataSource()
				},
				menus: DataAndFiltersService.getPrefilterMenuItems($this),
				dataSourceTypeControl: null,
				datasourceTypeControlLabel: null,
				argumentComponentpreFetchedData: null,
			};
		},
		computed: {
			showPrefilters: function () {
				return this.dataObject.dataSource && this.dataObject.dataSource.type &&
					ReportDataSourceService.get(this.dataObject.dataSource.type).usePreFilters;
			}
		},
		async created() {
			await this.displayControlByTypeAsync(this.dataObject.dataSource.type);

			await loadMessagesAsync(this, import.meta.glob('@/Modules/ReportAdmin/Components/Content/data-and-filters-messages.*.json'));
		},
		methods: {
			async dataSourceTypeValueChanged() {
				await this.displayControlByTypeAsync(this.dataObject.dataSource.type);
				if (this.dataSourceTypeControl.setDefaultDataSourceArgument)
					this.dataSourceTypeControl.setDefaultDataSourceArgument(this.dataObject.dataSource);
				else
					this.dataObject.dataSource.argument = null;
				this.$emit('datasource-changed', true);
			},
			validateAsync() {
				return Promise.resolve(this.$refs.dataAndFiltersForm.instance.validate().isValid);
			},
			async displayControlByTypeAsync(type) {
				if (type) {
					const reportDataSource = ReportDataSourceService.get(type);
					if (reportDataSource.preFetchDataAsync)
						this.argumentComponentpreFetchedData = await reportDataSource.preFetchDataAsync(this.dataObject.dataSource);
					this.dataSourceTypeControl = reportDataSource;
					if (this.dataSourceTypeControl)
						this.datasourceTypeControlLabel = this.dataSourceTypeControl.getDescriptionKey().label;
				}
			},
			onInitNewRow: function ($event) {
				$event.data.isRequired = false;
			},
			onRowValidating(e) {
				if (e.newData.invariantId) {
					if (this.dataObject.filters.find(x => x.invariantId == e.newData.invariantId)) {
						e.errorText = this.$t('multipleFilterValidationError');
						e.isValid = false;
					}
				}
			},
		},
		watch: {
			'dataObject.dataSource.argument'(newValue) {
				this.$emit('datasource-changed', false);
			},
		}
	}

</script>
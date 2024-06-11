<template>
	<page-container type="list">
		<box v-if="reportsDataSource">
			<dx-data-grid :data-source="reportsDataSource">
				<dx-toolbar>
					<dx-item name="addRowButton" 
							 location="before"
							 template="actionMenu"/>
				</dx-toolbar>
				<dx-filter-row :visible="true" />
				<dx-editing :allow-adding="true" />
				<dx-search-panel :visible="true" :width="250" />
				<dx-column width="200px"
						   type="buttons"
						   cell-template="actionsTemplate">
				</dx-column>
				<dx-column data-field="invariantId"
						   data-type="string"
						   :allow-filtering="true"
						   :caption="$t('reportCode')" />
				<dx-column data-field="name"
						   data-type="string"
						   :allow-filtering="true"
						   :caption="$t('reportName')" />
				<dx-column data-field="categoryInvariantId"
						   data-type="string"
						   :allow-filtering="true"
						   :caption="$t('reportCategory')"
						   :lookup="reportCategorySelectBoxOptions" />
				<template #actionMenu>
					<action-menu :items="actionList" menu-display-direction="right" show-menu-icon="add"></action-menu>
				</template>
				<template #actionsTemplate="cellInfo">
					<div>
						<router-link :to="{ name: 'EditReportDetails', params: { invariantId: cellInfo.data.data.invariantId } }"
									 class="button-spacing"
									 v-if="cellInfo.data.data.supportSave">{{ $t('edit') }}</router-link>
						<router-link :to="{ name: 'ManageViews', params: { invariantId: cellInfo.data.data.invariantId } }"
									 class="button-spacing"
									 v-if="cellInfo.data.data.supportSave">{{ $t('manageViews') }}</router-link>
						<a class="button-spacing" @click="exportReportJson(cellInfo.data.data.invariantId)">{{ $t('export') }}</a>
					</div>
				</template>
			</dx-data-grid>
		</box>
		<dx-popup ref="existingReportPopUp"
				  :close-on-outside-click="true"
				  :show-title="true"
				  :width="400"
				  :height="175"
				  title-template="popupTitle">
			<template #popupTitle>
				{{$t('newReportFromExisting')}}
			</template>
			<dx-select-box :data-source="reportsDataSource"
						   display-expr="name"
						   value-expr="invariantId"
						   v-model="selectedReportModelInvariantId">
			</dx-select-box>
			<div class="form-buttons">
				<dx-button type="success"
						   :disabled="!selectedReportModelInvariantId"
						   @click="goToNewReportDetailsFromExisting"
						   :text="$t('goToCreationPage')" />
			</div>
		</dx-popup>

	</page-container>
</template>

<script>
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
	import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";
	import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue";
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import { DxDataGrid, DxColumn, DxFilterRow, DxEditing, DxSearchPanel, DxToolbar, DxItem } from 'devextreme-vue/data-grid';
	import { ReportCategoriesDataSourceOptions } from "@/Modules/ReportMasterData/Services/report-category-master-data-service";
	import ActionMenu from "@UILibrary/Fwamework/ActionMenu/Components/ActionMenuComponent.vue";
	import { DxPopup } from 'devextreme-vue/popup';
	const existingReportPopUp = "existingReportPopUp";
	import { DxSelectBox } from 'devextreme-vue/select-box';
	import DxButton from "devextreme-vue/button";
	import ReportAdminService from '@/Modules/ReportAdmin/Services/report-admin-service';
	import { DefaultReportIcon } from "@/Modules/Reports/reports-module";

	export default {
		components: {
			PageContainer,
			Box,
			DxDataGrid,
			DxColumn,
			DxFilterRow,
			DxEditing,
			DxSearchPanel,
			ActionMenu,
			DxPopup,
			DxSelectBox,
			DxButton,
			DxToolbar,
			DxItem
		},
		data() {
			return {
				reportsDataSource: null,
				reportCategorySelectBoxOptions: {
					valueExpr: "invariantId",
					displayExpr: "name",
					dataSource: ReportCategoriesDataSourceOptions
				},
				actionList: [{
					icon: 'fas fa-plus',
					text: this.$t('newReport'),
					action: this.goToNewReport

				},
				{
					icon: 'fas fa-copy',
					text: this.$t('newFromExisting'),
					action: this.openFromExistingReportPopUp
				}
				],
				selectedReportModelInvariantId: null
			};
		},
		created: showLoadingPanel(async function () {
			await loadMessagesAsync(this, import.meta.glob('@/Modules/ReportAdmin/Content/report-admin-global-messages.*.json'));

			let reportList = await ReportAdminService.getAllAsync();
			reportList.forEach(r => r.icon = r.icon ?? DefaultReportIcon)
			this.reportsDataSource = reportList;
		}),
		methods: {
			async exportReportJson(invariantId) {
				await ReportAdminService.downloadRawJsonAsync(invariantId);
			},
			goToNewReportDetailsFromExisting() {
				if (this.selectedReportModelInvariantId)
					this.$router.push({ name: 'NewReportDetailsFromExisting', params: { invariantId: this.selectedReportModelInvariantId } });
			},
			goToNewReport() {
				this.$router.push({ name: 'NewReportDetails' });
			},
			openFromExistingReportPopUp() {
				this.$refs[existingReportPopUp].instance.show();
			}
		}
	}
</script>
<style>
	.button-spacing {
		margin-left: 5px;
	}
</style>
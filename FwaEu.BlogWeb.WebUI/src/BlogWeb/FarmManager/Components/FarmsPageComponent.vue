<template>
	<page-container type="list" v-if="hasSaveFarmPermission !== null">
		<box>
			<dx-data-grid :data-source="farmsDataSource"
						  width="100%"
						  ref="farmsGrid"
						  @init-new-row="onInitNewRow($event)"
						  @exporting="(e) => onExporting(e, $t('exportFileName'))">
				<dx-filter-row :visible="true" />
				<dx-search-panel :visible="true" :width="250" />
				<dx-editing :allow-adding="hasSaveFarmPermission" />
				<dx-export :enabled="true" />
				<dx-paging :page-size="35" />
				<dx-column width="80px"
						   type="buttons"
						   cell-template="detailsTemplate">
				</dx-column>
				<template #detailsTemplate="cellInfo">
					<router-link :to="{ name: 'FarmSummary', params: { id: cellInfo.data.data.id } }">{{ $t('edit') }}</router-link>
				</template>
				<dx-column data-field="name" :caption="$t('name')" :min-width="150" />
				<dx-column data-field="postalCodeId" :caption="$t('postalCode')" width="20%" :min-width="150" :lookup="postalCodeSelectBoxOptions" />
				<dx-column data-field="categorySize" :caption="$t('categorySize')" width="100px" :lookup="categorySizeSelectBoxOptions" />
				<dx-column data-field="mainActivityId" :caption="$t('principalActivity')" width="120px" :lookup="mainActivitySelectBoxOptions" />
				<dx-column data-field="sellingPriceInEurosWithoutTaxes" :caption="$t('sellingPrice')" width="100px" format="#,##0.00" />
				<dx-column data-field="recruitEmployees" :caption="$t('employeesRecruitement')" width="100px" cell-template="recruitEmployeesTemplate" />
				<dx-column data-field="openingDate" :caption="$t('openingDate')" cell-template="dateTemplate" width="100px" />
				<dx-column data-field="closingDate" :caption="$t('closingDate')" cell-template="dateTemplate" width="100px" />
				<dx-column data-field="animalCount" :caption="$t('animalCount')" width="100px" format="#,##0" />
				<dx-column data-field="updatedById" :caption="$t('modification')" cell-template="modificationTemplate" width="130px" :lookup="userSelectBoxOptions" />

				<template #recruitEmployeesTemplate="cellInfo">
					{{ getRecruitEmployeesText(cellInfo)}}
				</template>
				<template #modificationTemplate="cellInfo">
					<user-date :date="cellInfo.data.data.updatedOn" :user="getItemById('updatedById', cellInfo.data.data.updatedById)" />
				</template>
				<template #dateTemplate="cellInfo">
					<date-literal :date="cellInfo.data.value" :null-text="$t('nullClosingDateText')" />
				</template>
			</dx-data-grid>
		</box>
	</page-container>
</template>
<script>
	import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";

	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import PageContainer from '@/Fwamework/PageContainer/Components/PageContainerComponent.vue';
	import { DxDataGrid, DxPaging, DxColumn, DxExport, DxEditing, DxSearchPanel, DxFilterRow } from 'devextreme-vue/data-grid';
	import UserDate from "@/Fwamework/Users/Components/UserDateComponent.vue"
	import DateLiteral from '@UILibrary/Fwamework/Utils/Components/DateLiteralComponent.vue';

	import FarmsService from '@/BlogWeb/FarmManager/Services/farms-service';
	import { FarmCategorySizeDataSourceOptions } from "@/BlogWeb/FarmManager/Services/farm-category-sizes-master-data-service";

	import DataGridDataSourceFactory from "@UILibrary/Modules/MasterData/Services/data-grid-data-source-factory";
	import { FarmPostalCodeDataSourceOptions } from "@/BlogWeb/FarmManager/Services/farm-postal-code-master-data-service";
	import { FarmActivitiesDataSourceOptions } from "@/BlogWeb/FarmManager/Services/farm-activities-master-data-service";
	import { hasPermissionAsync } from "@/Fwamework/Permissions/Services/current-user-permissions-service";
	import { CanSaveFarms } from "@/BlogWeb/FarmManager/farms-permissions";
	import { UsersDataSourceOptions } from "@/Modules/UserMasterData/Services/users-master-data-service";
	import ViewContextService from '@/BlogWeb/ViewContext/Services/view-context-service';
	import { onExporting } from '@/DevExtreme/Extensions/Services/export-helper';

	export default {
		components: {
			DxDataGrid,
			DxPaging,
			DxColumn,
			DxExport,
			DxEditing,
			DxSearchPanel,
			DxFilterRow,

			Box,
			PageContainer,
			UserDate,
			DateLiteral
		},
		props: {
			onlyFarmsWithoutAnimals: {
				required: false,
				default: false
			}
		},
		data() {
			const $this = this;
			return {
				hasSaveFarmPermission: null,
				farms: [],
				postalCodeSelectBoxOptions: {
					displayExpr: function (postalCode) {
						return postalCode ? `${postalCode.postalCode} - ${postalCode.townName}` : "";
					},
					dataSource: FarmPostalCodeDataSourceOptions,
					valueExpr: 'id'
				},
				farmsDataSource: DataGridDataSourceFactory.createDataSource({
					getDataGrid: () => $this.$refs.farmsGrid,
					storeOptions: { load: $this.loadFarmsAsync }
				}),
				categorySizeSelectBoxOptions: {
					valueExpr: "id",
					displayExpr: "text",
					dataSource: FarmCategorySizeDataSourceOptions
				},
				mainActivitySelectBoxOptions: {
					valueExpr: "id",
					displayExpr: "name",
					dataSource: FarmActivitiesDataSourceOptions
				},
				userSelectBoxOptions: {
					dataSource: UsersDataSourceOptions,
					valueExpr: 'id',
					displayExpr: 'fullName'
				},
				selectedFarmRegionId: ViewContextService.get()?.regionId,
				viewContextChangeOff: ViewContextService.onChanged(() => {
					$this.$refs.farmsGrid.instance.refresh();

				})
			};
		},
		created: showLoadingPanel(async function () {
			await loadMessagesAsync(this, import.meta.glob('@/BlogWeb/FarmManager/Components/Content/farms-messages.*.json'));

			this.hasSaveFarmPermission = await hasPermissionAsync(CanSaveFarms);
		}),
		unmounted() {
			this.viewContextChangeOff();
		},
		methods: {
			onInitNewRow(e) {
				e.promise = this.$router.push({ name: 'CreateFarm' });
			},
			loadFarmsAsync: showLoadingPanel(async function () {
				return await FarmsService.getAllAsync(this.onlyFarmsWithoutAnimals);
			}),
			getRecruitEmployeesText(cellInfo) {
				return this.$t(cellInfo.data.value ? 'yes' : 'no');
			},
			getItemById(fieldName, id) {
				return this.$refs.farmsGrid.instance.columnOption(fieldName).lookup.items.find(i => i.id == id);
			},
			onExporting
		}
	};
</script>

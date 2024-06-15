<template>
	<div class="properties-component">
		<dx-data-grid :data-source="dataObject">
			<dx-paging :enable="false" />
			<dx-sorting mode="none" />
			<dx-filter-row :visible="false" />
			<dx-search-panel :visible="false" />
			<dx-column data-field="name"
					   data-type="string"
					   :caption="$t('name')" />
			<dx-column cell-template="fieldInvariantIdTemplate"
					   :caption="$t('fieldInvariantId')" />
			<template #fieldInvariantIdTemplate="{ data }">
				<div>
					<dx-select-box v-if="reportFields"
								   :data-source="reportFields"
								   display-expr="name"
								   value-expr="invariantId"
								   v-model="data.data.fieldInvariantId">
					</dx-select-box>
				</div>
			</template>
		</dx-data-grid>
	</div>
</template>

<script>
	import dxSelectBox from 'devextreme-vue/select-box';
	import {
		DxDataGrid, DxColumn, DxFilterRow, DxSearchPanel, DxPaging, DxSorting
	} from 'devextreme-vue/data-grid';

	import ReportFieldMasterDataService from "@/Modules/ReportMasterData/Services/report-field-master-data-service";
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";

	export default {
		components: {
			DxDataGrid,
			DxColumn,
			DxPaging,
			DxSorting,
			DxFilterRow,
			DxSearchPanel,
			dxSelectBox,
		},
		data() {
			return {
				reportFields: undefined,
			};
		},
		created: async function () {
			this.reportFields = await ReportFieldMasterDataService.getAllAsync();

			await loadMessagesAsync(this, import.meta.glob('@/Modules/ReportAdmin/Components/Content/properties.*.json'));
		},
		props: {
			dataObject: {
				type: Array,
				required: true
			},
		},
	}
</script>

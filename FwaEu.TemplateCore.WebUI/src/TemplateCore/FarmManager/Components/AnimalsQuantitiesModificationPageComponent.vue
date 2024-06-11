<template>
	<page-container type="list">
		<box v-if="animalCountDataSource">
			<dx-data-grid :data-source="animalCountDataSource"
						  width="100%" ref="animalCountGrid"
						  @row-inserted="saveOrUpdateAsync" 
						  @row-updated="saveOrUpdateAsync"
						  @exporting="(e) => onExporting(e, $t('exportFileName'))">
				<dx-search-panel :visible="true" :width="250" />
				<dx-editing :allow-adding="true" :allow-updating="true" />
				<dx-export :enabled="true" />
				<dx-paging :page-size="35" />
				<dx-column type="buttons" />
				<dx-column data-field="animalSpeciesId" :caption="$t('species')" :lookup="animalSpeciesOptions" />
				<dx-column data-field="quantity" :caption="$t('quantity')" />
				<dx-column data-field="updatedOn" :caption="$t('modification')" :allow-editing="false" cell-template="modificationTemplate" width="130px" />
				<template #modificationTemplate="cellInfo">
					<user-date :date="cellInfo.data.data.updatedOn" :userId="cellInfo.data.data.updatedById" />
				</template>
			</dx-data-grid>
		</box>
	</page-container>
</template>
<script>
	import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue";
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import UserDate from "@/Fwamework/Users/Components/UserDateComponent.vue"
	import { DxDataGrid, DxPaging, DxColumn, DxExport, DxEditing, DxSearchPanel } from 'devextreme-vue/data-grid';
	import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";
	import FarmAnimalsCountService from "@/TemplateCore/FarmManager/Services/farm-animals-count-service.js";
	import { AsyncLazy } from '@/Fwamework/Core/Services/lazy-load';
	import FarmGeneralInformationService from "@/TemplateCore/FarmManager/Services/farm-general-information-service";
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
	import { FarmAnimalSpeciesDataSourceOptions } from "../Services/farm-animal-species-master-data-service";
	import { onExporting } from '@/DevExtreme/Extensions/Services/export-helper';

	export default {
		components: {
			PageContainer,
			Box,
			DxDataGrid,
			DxPaging,
			DxExport,
			DxColumn,
			DxEditing,
			DxSearchPanel,
			UserDate
		},
		data() {
			let $this = this;
			return {
				animalSpeciesOptions: {
					valueExpr: "id",
					displayExpr: "name",
					dataSource: FarmAnimalSpeciesDataSourceOptions
				},
				farmLazy: new AsyncLazy(() => FarmGeneralInformationService.getAsync($this.$route.params.id)),
				animalCountDataSource: null

			}
		},
		created: showLoadingPanel(async function () {
			await loadMessagesAsync(this, import.meta.glob('@/TemplateCore/FarmManager/Components/Content/animal-quantities-messages.*.json'));
			await this.loadAnimalsCountAsync();
		}),
		methods: {
			async loadAnimalsCountAsync() {
				let farmId = this.$route.params.id;
				this.animalCountDataSource = await FarmAnimalsCountService.getAllByFarmIdAsync(farmId);
			},
			async getCurrentFarmAsync() {
				return await this.farmLazy.getValueAsync();
			},
			saveOrUpdateAsync: showLoadingPanel(async function () {
				await FarmAnimalsCountService.saveOrDeleteByFarmIdAsync(this.$route.params.id, this.animalCountDataSource);
				await this.loadAnimalsCountAsync();
			}),
			onExporting
		}
	}

</script>
<style scoped>
	.page-container {
		max-width: 700px;
	}
</style>
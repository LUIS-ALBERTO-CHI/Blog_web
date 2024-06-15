<template>
	<box :title="$t('animals')" :menu-items="menuButtons">
		<!-- TODO: Display the quantity of different types of animals https://dev.azure.com/fwaeu/BlogWeb/_workitems/edit/4931-->

		<dx-data-grid :data-source="dataSource">
			<dx-column data-field="animalSpeciesId" :caption="$t('animalSpecies')" :lookup="animalSpeciesOptions" />
			<dx-column data-field="quantity" :caption="$t('quantity')" />
		</dx-data-grid>
	</box>
</template>
<script>
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import { DxDataGrid, DxColumn } from 'devextreme-vue/data-grid';
	import { hasPermissionAsync } from '@/Fwamework/Permissions/Services/current-user-permissions-service';
	import { CanSaveFarms } from '@/BlogWeb/FarmManager/farms-permissions';
	import { FarmAnimalSpeciesDataSourceOptions } from "@/BlogWeb/FarmManager/Services/farm-animal-species-master-data-service";

	export default {
		components: {
			Box,
			DxDataGrid,
			DxColumn
		},
		props: {
			dataSource: {
				type: Array,
				required: true
			}
		},
		data() {
			return {
				menuButtons: [],
				animalSpeciesOptions: {
					valueExpr: "id",
					displayExpr: "name",
					dataSource: FarmAnimalSpeciesDataSourceOptions
				}
			}
		},
		async created() {
			await loadMessagesAsync(this, import.meta.glob('@/BlogWeb/FarmManager/Components/Content/summary-animals-count-messages.*.json'));

			let $this = this;
			if (await hasPermissionAsync(CanSaveFarms)) {
				this.menuButtons = [{
					text: $this.$t('modifyQuantities'),
					icon: "edit",
					action() {
						$this.$router.push({ name: 'AnimalsQuantities', params: { id: $this.$route.params.id } });
					}
				}]
			}
		},
	}
</script>
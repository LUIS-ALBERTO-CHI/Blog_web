<template>
	<page-container type="summary" custom-class="report-menu">
		<box v-if="menuItemsDataSource">
			<menu-list :menu-items="menuItemsDataSource" />
		</box>
	</page-container>
</template>

<script>
	import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue";
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import { showLoadingPanel } from '@/Fwamework/LoadingPanel/Services/loading-panel-service';
	import ReportService from '@/Modules/Reports/Services/reports-service';
	import MenuList from '@UILibrary/Fwamework/Menu/Components/MenuListComponent.vue';
	import ReportMasterData from "@/Modules/ReportMasterData/Services/report-category-master-data-service";
	import { DefaultReportIcon } from '@/Modules/Reports/reports-module';

	export default {

		components: {

			PageContainer,
			Box,
			MenuList
		},

		data() {
			return {
				menuItemsDataSource: null
			}
		},

		created: showLoadingPanel(async function () {
			let allReports = await ReportService.getAllAsync();
			let categories = await ReportMasterData.getAllAsync();
			let allReportsItems = allReports.filter(report => report.model.navigation.summary?.visible ?? false)
				.map(report => {
					let category = categories?.find(c => c.invariantId == report.model.categoryInvariantId);
					let categoryName = category?.name ?? this.$t("defaultCategory");
					let categoryDescription = category?.description;
					return {
						text: report.model.name,
						icon: report.model.icon ?? DefaultReportIcon,
						descriptionText: report.model.description,
						path: report.route,
						category: categoryName,
						categoryDescription: categoryDescription
					}
				});

			let groupedItems = [];
			for (let menuItem of allReportsItems) {
				let groupItem = groupedItems.find(gi => gi.text === menuItem.category);
				if (!groupItem) {
					groupItem = { text: menuItem.category, items: [], descriptionText: menuItem.categoryDescription };
					groupedItems.push(groupItem);
				}
				groupItem.items.push(menuItem);
			}
			this.menuItemsDataSource = groupedItems;
		})
	}
</script>

<style src="../Content/report-menu.css"></style>

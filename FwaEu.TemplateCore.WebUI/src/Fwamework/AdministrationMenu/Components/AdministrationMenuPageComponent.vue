<template>
	<page-container type="summary" custom-class="administration-menu">
		<box v-if="menuItemsDataSource">
			<menu-list :menu-items="menuItemsDataSource" />
		</box>
	</page-container>
</template>

<script>
	import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue";
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import { showLoadingPanel } from '@/Fwamework/LoadingPanel/Services/loading-panel-service';
	import AdministrationMenuService from '@/Fwamework/AdministrationMenu/Services/administration-menu-service';
	import MenuList from '@UILibrary/Fwamework/Menu/Components/MenuListComponent.vue';

	export default {

		components: {

			PageContainer,
			Box,
			MenuList
		},

		methods: {
			sortAdminGroups(a, b) {
				const groupA = a.text.toUpperCase();
				const groupB = b.text.toUpperCase();

				if (a.groupIndex > b.groupIndex) return 1;
				if (a.groupIndex < b.groupIndex) return -1;

				return groupA.localeCompare(groupB);
			}
		},

		data() {
			return {
				menuItemsDataSource: null
			}
		},

		created: showLoadingPanel(async function () {
			const allMenuItems = await AdministrationMenuService.getMenuItemsAsync();
			let groupedItems = [];
			for (let menuItem of allMenuItems) {
				let groupItem = groupedItems.find(gi => gi.groupKey === menuItem.groupKey);
				if (!groupItem) {
					groupItem = {
						text: menuItem.groupText,
						groupKey: menuItem.groupKey,
						items: [],
						groupIndex: menuItem.groupIndex
					};
					groupedItems.push(groupItem);
				}
				groupItem.items.push(menuItem);
			}

			groupedItems.sort((a, b) => this.sortAdminGroups(a, b));
			this.menuItemsDataSource = groupedItems;
		})
	}
</script>
<style src="../Content/administration-menu.css"></style>
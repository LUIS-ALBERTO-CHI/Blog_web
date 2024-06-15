<template>
	<page-container type="form">
		<box>
			Test multiple menu
			<action-menu :items="menus" menu-display-direction="right" />
			Test single menu
			<action-menu :items="singleItem" />
			Test single menu (force menu mode off)
			<action-menu :items="singleItem" :force-menu-mode="false" menu-display-direction="right" />
			Test multiple button actions (force menu mode off)
            <action-menu :items="buttonActions" :force-menu-mode="false" menu-display-direction="right" />
		</box>
		<box>
			Test multiple menu
			<prime-action-menu :items="menuItems" menu-display-direction="right" />
			Test single menu
			<prime-action-menu :items="singleMenuItem" />
			Test single menu (force menu mode off)
			<prime-action-menu :items="singleMenuItem" :force-menu-mode="false" menu-display-direction="right" />
			Test multiple button actions (force menu mode off)
			<prime-action-menu :items="buttonActionsMenu" :force-menu-mode="false" menu-display-direction="right" />
		</box>
	</page-container>

</template>

<script>
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import ActionMenu from "@/DevExtreme/Fwamework/ActionMenu/Components/ActionMenuComponent.vue";
	import PrimeActionMenu from "@/PrimeVue/Fwamework/ActionMenu/Components/ActionMenuComponent.vue";
	import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue";
	import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";

	export default {
		components: {
			Box,
			ActionMenu,
			PageContainer,
			PrimeActionMenu
		},
		data() {
			const
				menus = [
				{
						icon: 'fa-solid fa-globe',
					text: 'Cafe',
					action: 'https://google.com'
				},
				{
					icon: 'fa-solid fa-mug-saucer',
					text: 'Map',
					action: () => { alert("Test action comme fonction") },
					color: '#fc0303'
				},
				{
					label: 'Vue Website',
					text: 'Vue Website',
					icon: 'fa-solid fa-globe',
					action: 'https://vuejs.org/'
				},
				{
					icon: 'fa-solid fa-upload',
					text: 'Plane',
					action: '/Notification'
				}
			];
			const menuItems = [
				{
					icon: 'fa-solid fa-mug-saucer',
					text: 'Cafe',
					action: 'https://google.com'
				},
				{
					icon: 'fa-solid fa-globe',
					text: 'Map',
					action: () => { alert("Test action comme fonction") },
					color: '#fc0303'
				},
				{
					text: 'Vue Website',
					icon: 'fa-solid fa-globe',
					action: 'https://vuejs.org/'
				},
				{
					text: 'Router',
					icon: 'fa-solid fa-upload',
					to: '/Notification'
				}
			];

			return {
				menus, menuItems,
				singleItem: [menus[0]],
				singleMenuItem: [menuItems[0]],
				buttonActionsMenu: [menuItems[0], menuItems[1]],
				buttonActions: [menus[0], menus[1]]

			};
		},
		created: showLoadingPanel(async function () {

		}),
	}
</script>

<template>
	<div class="toolbar">
		<div class="toolbar-action-menu" v-if="menuItems">
			<action-menu v-bind="menuOptions" :items="menuItems"></action-menu>
		</div>
		<div class="toolbar-content">
			<slot />
		</div>
		<div class="toolbar-clear" />
	</div>
</template>
<script>
	import ActionMenu from "@UILibrary/Fwamework/ActionMenu/Components/ActionMenuComponent.vue";

	export default {
		components: {
			ActionMenu
		},
		props: {
			menuItems: {
				type: Array,
				required: false,
				default: null
			},
			menuOptions: {
				type: Object,
				required: false,
				default: null
			}
		}
	}

</script>
<style src="./Content/toolbar.css">
</style>
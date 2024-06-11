<template>
	<div class="user-panel">
		<div class="user-info">
			<div class="image-container">
				<user-avatar :user="user" size="medium" />
			</div>
			<div class="user-name">{{user.fullName}}</div>
		</div>

		<dx-context-menu v-if="menuMode === 'context'"
						 target=".user-button"
						 :items="menuItems"
						  @item-click="onItemClick"
						 :width="170"
						 :position="menuPositionConfig"
						 show-event="dxclick"
						 css-class="user-menu" />

		<dx-list v-if="menuMode === 'list'"
				 class="dx-toolbar-menu-action"
				  @item-click="onItemClick"
				 :items="menuItems" />
	</div>
</template>

<script>
	import DxContextMenu from "devextreme-vue/context-menu";
	import DxList from "devextreme-vue/list";
	import UserAvatar from '@UILibrary/Fwamework/Users/Components/UserAvatarComponent.vue'

	export default {
		props: {
			menuMode: String,
			user: Object,
			menuItems: Array
		},
		components: {
			DxContextMenu,
			DxList,
			UserAvatar
		},
		data() {
			return {
				menuPositionConfig: {
					my: "top center",
					at: "bottom center"
				}
			};
		},
		methods: {
			onItemClick(e) {
				if (e.itemData.path) {
					this.$router.push(e.itemData.path);
				}
			}
		}
	};
</script>

<style lang="scss">

	@import "@UILibrary/Extensions/Themes/generated/variables.base.scss";

	.user-info {
		display: flex;
		align-items: center;
		.dx-toolbar-menu-section &

	{
		padding: 10px 6px;
		border-bottom: 1px solid rgba(0, 0, 0, 0.1);
	}

	.image-container {
		margin-right: 5px;
	}

	.user-name {
		font-size: 14px;
		color: $base-text-color;
	}

	}

	.user-panel {
		.dx-list-item .dx-icon

	{
		vertical-align: middle;
		color: rgba(0, 0, 0, 0.87);
		margin-right: 16px;
	}

	.dx-rtl .dx-list-item .dx-icon {
		margin-right: 0;
		margin-left: 16px;
	}

	}

	.dx-context-menu.user-menu.dx-menu-base {
		&.dx-rtl

	{
		.dx-submenu .dx-menu-items-container .dx-icon

	{
		margin-left: 16px;
	}

	}

	.dx-submenu .dx-menu-items-container .dx-icon {
		margin-right: 16px;
	}

	.dx-menu-item .dx-menu-item-content {
		padding: 3px 15px 4px;
	}

	}

	.dx-theme-generic .user-menu .dx-menu-item-content .dx-menu-item-text {
		padding-left: 4px;
		padding-right: 4px;
	}
</style>

<template>
	<div class="user-header">
		<div class="user-info" v-if="user" @click="toggleNotificationPanel">
			<div class="image-container">
				<user-avatar :user="user" size="medium" />
			</div>
			<div class="user-name" v-show="!smallModeEnabled">{{user.fullName}}</div>
		</div>

		<dx-popover v-if="!smallModeEnabled"
					v-model:visible="isNotificationPanelVisible"
					target=".user-header"
					:position="menuPositionConfig">
			<dx-menu class="user-header-menu"
					 orientation='vertical'
					 :items="menuItems"
					 @item-click="onItemClick"
					 show-event="dxclick"
					 css-class="user-menu" />
		</dx-popover>
	</div>
</template>

<script>
	import UserAvatar from '@/Fwamework/Users/Components/UserAvatarComponent.vue';
	import DxPopover from 'devextreme-vue/popover';
	import DxMenu from 'devextreme-vue/menu';

	export default {
		components: {
			UserAvatar,
			DxPopover,
			DxMenu
		},
		props: {
			smallModeEnabled: Boolean,			
			fetchedData: {
				required: true,
				type: Object
			}
		},
		data() {
			return {
				user: this.fetchedData.currentUser,
				menuItems: this.fetchedData.userMenuItems,
				menuPositionConfig: {
					my: "top center",
					at: "bottom center"
				},
				isNotificationPanelVisible: false,
			};
		},
		methods: {
			onItemClick(e) {
				if (e.itemData.path) {
					this.$router.push(e.itemData.path);
					this.isNotificationPanelVisible = false;
				}
			},
			toggleNotificationPanel() {
				this.isNotificationPanelVisible = !this.isNotificationPanelVisible;
			},
		}
	}
</script>

<style lang="scss">
	@import "@UILibrary/Extensions/Themes/generated/variables.base.scss";

	.user-header-menu.dx-menu .dx-menu-item {
		height: auto;

		.dx-icon {
			color: var(--secondary-text-color)
		}

		.dx-menu-item-text {
			padding: 10px 12px;
		}
	}

	.screen-x-small .user-header .user-name {
		display: none;
	}

	.user-info {
		display: flex;
		align-items: center;

		.dx-toolbar-menu-section & {
			padding: 10px 6px;
			border-bottom: 1px solid rgba(0, 0, 0, 0.1);
		}

		.image-container {
			margin-right: 5px;
		}

		.user-name {
			flex-shrink: 0;
			font-size: 14px;
			color: $base-text-color;
		}
	}
</style>
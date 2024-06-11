<template>
	<div class="user-header">
		<div class="user-info" v-if="user" @click="toggleNotificationPanel">
			<div class="image-container">
				<user-avatar :user="user" size="medium" />
			</div>
			<div class="user-name" v-show="!smallModeEnabled">{{user.fullName}}</div>
		</div>
		<OverlayPanel v-if="!smallModeEnabled" ref="opanel" :dismisable="true" :baseZIndex="1013">
			<Menu :model="opMenuItems">
					
			</Menu>
		</OverlayPanel>
		
	</div>
</template>

<script>
	import OverlayPanel from 'primevue/overlaypanel';
	import Menu from 'primevue/menu';
	import UserAvatar from '@/Fwamework/Users/Components/UserAvatarComponent.vue';

	export default {
		components: {
			UserAvatar,
			OverlayPanel,
			Menu
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
				menuItems: this.fetchedData.userMenuItems
			};
		},
		methods: {
			toggleNotificationPanel(event) {
				if (!this.smallModeEnabled) {
					this.$refs.opanel.toggle(event);
				}
				
			},
		},
		computed: {
			opMenuItems() {
				return this.fetchedData.userMenuItems.map((x) => {
					return {
						label: x.text,
						command: x.onClick,
						to: x.path,
						...x
					};
				});
			},
		}
	}
</script>

<style lang="scss">
	.p-menu .p-menuitem > .p-menuitem-content .p-menuitem-link .p-menuitem-icon {
		color: var(--secondary-text-color);
	}

	.p-menuitem-icon {
		
		width: 24px;
	}

	.p-overlaypanel .p-overlaypanel-content {
		padding: 0px;
	}

	.screen-x-small .user-header .user-name {
		display: none;
	}
	.user-info {
		display: flex;
		align-items: center;
	}

	.image-container {
		margin-right: 5px;
	}

	.user-name {
		flex-shrink: 0;
		font-size: 14px;
	}

	.p-menu {
		border:none;
	}
</style>
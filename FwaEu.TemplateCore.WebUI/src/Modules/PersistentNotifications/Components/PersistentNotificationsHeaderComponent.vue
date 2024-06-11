<template>
	<div>
		<div class="user-notification-icon" @click="toggleNotificationPanel">
			<i class="fas fa-bell alert-icon" />
			<notification-counter-badge :value="counter">
			</notification-counter-badge>
		</div>
		<persistent-notifications v-if="!smallModeEnabled"
								  :notifications-list="notificationsList"
								  :is-small-mode="smallModeEnabled"
								  :is-notification-panel-visible="isNotificationPanelVisible"
								  @notified-async="onNotifiedAsync"
								  @click-outside-popover="toggleNotificationPanel"/>
	</div>
</template>

<script>
	import NotificationDataService from "@/Modules/PersistentNotifications/Services/persistent-notifications-service";
	import PersistentNotifications from "@UILibrary/Modules/PersistentNotifications/Components/PersistentNotificationsComponent.vue";
	import NotificationCounterBadge from "@/Fwamework/NotificationCounterBadge/Components/NotificationCounterBadgeComponent.vue";

	export default {
		components: {
			PersistentNotifications,
			NotificationCounterBadge
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
				notificationsList: this.$props.fetchedData.notificationsList,
				isNotificationPanelVisible: false
			};
		},
		methods: {
			toggleNotificationPanel() {
				this.isNotificationPanelVisible = !this.isNotificationPanelVisible;
				if (this.counter > 0) {
					let notificationMaxDate = this.notificationsList.reduce((a, b) => {
						return new Date(a.sentOn) > new Date(b.sentOn) ? a : b;
					});
					this.delayBeforemarkAsSeenAsync().then(() => this.markAsSeenAsync(notificationMaxDate.sentOn));
				}
			},
			async delayBeforemarkAsSeenAsync() {
				return new Promise(resolve => setTimeout(resolve, 6000));
			},
			async markAsSeenAsync(date) {
				await NotificationDataService.markAsSeenAsync(date);
				for (let notif of this.notificationsList.filter((obj => obj.seenOn == null)))
					notif.seenOn = new Date(date);
			},
			async onNotifiedAsync(e) {
				this.delayBeforemarkAsSeenAsync().then(() => this.markAsSeenAsync(e.content.sentOn));
			}
		},
		computed: {
			counter() {
				return this.notificationsList.filter(item => item.seenOn == null).length;
			},
		},
	}
</script>

<style scoped>
	.user-notification-icon {
		margin-top: 4px;
		padding-bottom: 2px;
		position: relative;
	}

	.alert-icon {
		font-size: 24px;
		color: var(--secondary-text-color);
	}

	.user-notification-icon .item-counter {
		left: 18px;
		top: -8px;
	}
</style>
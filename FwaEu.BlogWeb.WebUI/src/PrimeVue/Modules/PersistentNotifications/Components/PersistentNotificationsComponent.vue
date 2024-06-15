<template>
	<div class="user-notification-header">

		<OverlayPanel v-if="!smallModeEnabled"
					:key="notificationsToDisplay.length"
					:width="smallModeEnabled ? '100%' : null"
					:max-width="smallModeEnabled ? null : '300px'"
					:visible="isNotificationPanelVisible"
					target=".user-notification-header"
					@hidden="onClickOutside">

			<div class="user-notification-result-item">
				<span v-show="notificationsToDisplay.length<=0"> {{ $t('noNotificationText') }} </span>
				<div v-for="notification in notificationsToDisplay" v-bind:Key="notification.id" class="notification-item-wrapper">
					<notification-item :notification="notification" @delete-notification="deleteNotificationAsync" :is-small-mode="smallModeEnabled" />
				</div>
			</div>
		</OverlayPanel>
		<div v-else
			 class="user-notification-result-item user-notification-result-item-mobile"
			 v-click-outside="hideSmallModeContent">
			<span v-show="notificationsToDisplay.length<=0"> {{ $t('noNotificationText') }} </span>
			<div v-for="notification in notificationsToDisplay" v-bind:Key="notification.id" class="notification-item-wrapper">
				<notification-item :notification="notification" @delete-notification="deleteNotificationAsync" :is-small-mode="smallModeEnabled" />
			</div>
		</div>
	</div>
</template>
<script>
	import OverlayPanel from 'primevue/overlaypanel';
	import NotificationItem from '@UILibrary/Modules/PersistentNotifications/Components/NotificationComponent.vue';
	import NotificationDataService from "@/Modules/PersistentNotifications/Services/persistent-notifications-service";
	import PersistentNotificationTypesService from "@/Modules/PersistentNotifications/Services/persistent-notification-type-service";
	import UserNotificationService from "@/Modules/UserNotifications/Services/user-notification-service";
	import PersistentNotificationModel from "@/Modules/PersistentNotifications/Services/persistent-notifications-model";
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";

	export default {
		components: {
			OverlayPanel,
			NotificationItem
		},
		props: {
			smallModeEnabled: Boolean,
			notificationsList: {
				required: true,
				type: Array
			},
			isNotificationPanelVisible: Boolean,
		},
		data() {
			return {
				notificationsToDisplay: this.notificationsList,
				persistentNotificationTypes: [],
				onNotifiedOff: UserNotificationService.onNotified(this.onNotifiedAsync),
			}
		},
		async created() {
			await loadMessagesAsync(this, import.meta.glob('@/Modules/PersistentNotifications/Components/Content/persistent-notifications-messages.*.json'));
			this.persistentNotificationTypes = await PersistentNotificationTypesService.getAllAsync();
		},
		methods: {
			onClickOutside() {
				if (this.isNotificationPanelVisible) {
					this.$emit('click-outside-popover');
				}
			},
			hideSmallModeContent() {
				this.$emit('hide-content');
			},
			//NOTE: Loading new notification using SignalR
			async onNotifiedAsync(e) {
				const persistentNotifType = this.persistentNotificationTypes.find(item => item.notificationType == e.notificationType);
				if (persistentNotifType)
					this.notificationsToDisplay.unshift(new PersistentNotificationModel(e.content.id, e.notificationType, e.content.sentOn,
						e.content.seenOn, await persistentNotifType.getMessageAsync(e.content.model), e.content.isSticky));
				if (this.isNotificationPanelVisible)
					this.$emit('on-notified-async', e);
			},
			async deleteNotificationAsync(id) {
				await NotificationDataService.deleteAsync(id);
				this.notificationsToDisplay.splice(this.notificationsToDisplay.findIndex((obj => obj.id == id)), 1);
			}
		},
		beforeUnmount() {
			this.onNotifiedOff();
		},
		watch: {
			notificationsList() {
				this.notificationsToDisplay = this.notificationsList;
			}
		}
	}
</script>
<style type="text/css" src="@/Modules/PersistentNotifications/Components/Content/persistent-notifications.css"></style>
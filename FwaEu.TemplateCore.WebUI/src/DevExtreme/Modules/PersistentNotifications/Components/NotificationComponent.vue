<template>
    <div :class="[{'user-notification-item-seen' : isNotificationSeen, 'button-delete-visible' : displayDeleteButton}, 'user-notification-item']"
		 @mouseover="showDeleteButton"
		 @mouseleave="hideDeleteButton">
        <div class="user-notification-item-title">
            <div class="user-notification-item-text">
                {{notification.model}}
            </div>
            <dx-button @click="deleteNotification" icon="clear" class="button-delete">
            </dx-button>
        </div>
        <div class="user-notification-item-date">
            {{ $t('sentOn') }}  {{ notification.sentOn.toLocaleString() }}
        </div>
    </div>
</template>
<script>
	import DxButton from 'devextreme-vue/button';
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";

    export default {
        components: {
            DxButton
		},
		props: {
			smallModeEnabled: Boolean,
            notification: {
                required: true
            }
		},
		data() {
			return {
				displayDeleteButton: false
			}
		},
		async created() {
			await loadMessagesAsync(this, import.meta.glob('@/Modules/PersistentNotifications/Components/Content/persistent-notifications-messages.*.json'));
			this.displayDeleteButton = this.smallModeEnabled && !this.notification.isSticky;
		},
        methods: {
            deleteNotification() {
                this.$emit('delete-notification', this.notification.id);
			},
			showDeleteButton() {
				if (!this.smallModeEnabled && !this.notification.isSticky)
					this.displayDeleteButton = true;
			},
			hideDeleteButton() {
				if (!this.smallModeEnabled)
					this.displayDeleteButton = false;
			}
		},
        computed: {
			isNotificationSeen() {
                return this.notification.seenOn != null;
			},
			deleteButtonVisibility() {
				return this.notification.isSticky ? 'hidden' : 'visible';
			}
        }
	}
</script>
<style type="text/css" src="@/Modules/PersistentNotifications/Components/Content/persistent-notifications.css"></style>
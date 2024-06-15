import { defineAsyncComponent } from "vue";

export default {
	key: "PersistentNotificationsHeaderItem",
	component: defineAsyncComponent(() => import('@/Modules/PersistentNotifications/Components/PersistentNotificationsHeaderComponent.vue')),
	smallModeContentComponent: defineAsyncComponent(() => import('@/Modules/PersistentNotifications/Components/PersistentNotificationsHeaderSmallModeContentComponent.vue')),

	async fetchDataAsync() {
		const persistentNotificationItemLoadService = (await import('@/Modules/PersistentNotifications/Services/persistent-notifications-items-load-service')).default;

		const data = {
			notificationsList: await persistentNotificationItemLoadService.getAllAsync()
		};

		return data;
	}
}
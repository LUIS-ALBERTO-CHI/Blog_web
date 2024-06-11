const AdministrationMenuPageComponent = () => import('@/Fwamework/AdministrationMenu/Components/AdministrationMenuPageComponent.vue');

export default [
	{
		path: '/Administration',
		name: 'Administration',
		component: AdministrationMenuPageComponent,
		meta: {
			zoneName: 'admin',
			breadcrumb: {
				titleKey: 'administration',
				parentName: 'default'
			}
		},
		beforeEnter: async (to, from, next) => {
			
			const menuService = (await import('@/Fwamework/AdministrationMenu/Services/administration-menu-service')).default;
			let isAnyMenuItemAvailable = await menuService.anyMenuItemAvailableAsync();
			
			if (isAnyMenuItemAvailable) {
				next();
			} else {
				//Cancel the current navigation if no items are available
				const notificationService = (await import('@/Fwamework/Notifications/Services/notification-service')).default;
				const i18n = (await import('@/Fwamework/Culture/Services/localization-service')).I18n;
				
				notificationService.showError(i18n.t("forbiddenAccessMessage"));
				next("/");
			}
		}
	}
]
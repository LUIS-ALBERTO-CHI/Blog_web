import Router from '@/Fwamework/Routing/Services/vue-router-service';

export default {
	configure() {

		Router.beforeEach(async (to, from, next) => {

			if (to.matched.some(record => record.meta && !record.meta.allowAnonymous && record.meta.requiredPermissions)) {
				const { hasPermissionsAsync, getAccessDeniedForUserByAnyPermissionMessageAsync, CheckOperation } = (await import('@/Fwamework/Permissions/Services/current-user-permissions-service'));

				for (let matchedRoute of to.matched) {
					const requiredPermissions = matchedRoute.meta.requiredPermissions;
					const requiredPermissionsCheckOperation = matchedRoute.meta.requiredPermissionsCheckOperation || CheckOperation.All;
					const isAuthorized = await hasPermissionsAsync(requiredPermissions, requiredPermissionsCheckOperation);

					if (!isAuthorized) {
						const notificationService = (await import('@/Fwamework/Notifications/Services/notification-service')).default;
						const errorMessage = await getAccessDeniedForUserByAnyPermissionMessageAsync(requiredPermissions);
						notificationService.showError(errorMessage);
						next(false);//Abort current navigation
						return;
					}
				}
			}
			next();
		});
	}
}
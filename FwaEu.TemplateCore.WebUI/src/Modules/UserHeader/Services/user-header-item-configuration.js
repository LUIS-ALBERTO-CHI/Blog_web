import { defineAsyncComponent } from 'vue';

export default {
	key: "CurrentUserHeaderItem",
	component: defineAsyncComponent(() => import('@UILibrary/Modules/UserHeader/Components/UserHeaderComponent.vue')),
	smallModeContentComponent: defineAsyncComponent(() => import('@UILibrary/Modules/UserHeader/Components/UserHeaderSmallModeContentComponent.vue')),

	async fetchDataAsync() {
		const currentUserService = (await import('@/Fwamework/Users/Services/current-user-service')).default;
		const currentUserMenuService = (await import('@/Fwamework/CurrentUserMenu/Services/current-user-menu-service')).default;
		const usersMasterDataService = (await import('@/Modules/UserMasterData/Services/users-master-data-service')).default;

		const data = {
			userMenuItems: await currentUserMenuService.getMenuItemsAsync(),
			currentUser: null
		};

		const user = await currentUserService.getAsync();
		if (user) {
			const masterData = await usersMasterDataService.getAsync(user.id);

			data.currentUser = {
				id: user.id,
				firstName: masterData.firstName,
				lastName: masterData.lastName,
				fullName: masterData.fullName,
				avatarUrl: masterData.avatarUrl
			};
		}
		return data;
	}
}
import Router from '@/Fwamework/Routing/Services/vue-router-service';

export default {
	configureAsync() {
		Router.beforeEach(async (to, from, next) => {
			const authenticationService = (await import('@/Fwamework/Authentication/Services/authentication-service')).default;
			const isCurrentUserAuthenticated = await authenticationService.isAuthenticatedAsync();
			if (isCurrentUserAuthenticated) {
				//Don't refresh master data if navigation target is error page
				//This could produce an infinite loop if error commes from master data refresh task
				if (to.name !== "Error") {
					const masterDataManager = (await import('@/Fwamework/MasterData/Services/master-data-manager-service')).default;
					await masterDataManager.refreshMasterDataIfNeededAsync();
				}
			}
			next();
		});
    }
}
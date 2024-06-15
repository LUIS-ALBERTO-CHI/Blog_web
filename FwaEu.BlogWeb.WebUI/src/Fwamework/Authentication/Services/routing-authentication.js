import Router from '@/Fwamework/Routing/Services/vue-router-service';
import AuthenticationService from './authentication-service';

const getRedirectUrl = function(route) {
	return route.query?.redirect ?? { name: "default" };
};

export default {
	configure() {

		AuthenticationService.onLoggedOut(async () => await Router.push({ name: "Login", query: { redirect: Router.currentRoute.value.fullPath } }));
		AuthenticationService.onLoggedIn(async () => await Router.push(getRedirectUrl(Router.currentRoute.value)));

		Router.beforeEach(async (to, from, next) => {
			const authenticationService = (await import('@/Fwamework/Authentication/Services/authentication-service')).default;
			
			if (to.name === "Login" && await authenticationService.isAuthenticatedAsync()) {
				let redirectTo = getRedirectUrl(to);
				next(redirectTo);
			}
			else if (to.matched.some(record => !record.meta.allowAnonymous) && !await authenticationService.isAuthenticatedAsync()) {
				next({
					name: "Login",
					query: { redirect: to.fullPath }
				});
			} else {
				next();
			}
		});
	}
}
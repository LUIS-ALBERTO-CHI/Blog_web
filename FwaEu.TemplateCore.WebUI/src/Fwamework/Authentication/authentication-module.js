import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';
import { CurrentUserMenuType } from '@/Fwamework/CurrentUserMenu/Services/current-user-menu-service';

export class AuthenticationModule extends AbstractModule {

	options = {
		enableLogout: true
	}
	constructor(options) {
		super();
		if (!options?.authenticationHandlers)
			throw new Error("Missing required option authenticationHandlers");
		
		this.options = { ...this.options, ...options };
	}

	async onInitAsync(vueApp) {
		const authenticationService = (await import('./Services/authentication-service')).default;
		await authenticationService.setAuthenticationHandlers(this.options.authenticationHandlers);

		const httpAuthenticationService = (await import('./Services/http-authentication')).default;
		httpAuthenticationService.configure();

		const routingAuthenticationService = (await import('./Services/routing-authentication')).default;
		routingAuthenticationService.configure();
	}

	async getMenuItemsAsync(menuType) {
		const menuItems = [];

		if (menuType === CurrentUserMenuType && this.options.enableLogout) {
			menuItems.push({
				textKey: "logout",
				icon: "fad fa-portal-exit",
				visibleIndex: 90000,
				async onClick() {
					const authenticationService = (await import('./Services/authentication-service')).default;
					await authenticationService.logoutAsync();
				}
			});
		}
		return menuItems;
	}
}

import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";
import { CurrentUserMenuType } from "@/Fwamework/CurrentUserMenu/Services/current-user-menu-service";
import UserSettingsPartRegistry from '@/Fwamework/UserSettings/Services/user-settings-parts-registry';
export class UserSettingsModule extends AbstractModule {

	async getMenuItemsAsync(menuType) {
		const menuItems = [];

		if (menuType === CurrentUserMenuType) {
			const userSettingsParts = UserSettingsPartRegistry.getAll();

			if (userSettingsParts.length > 0) {
				menuItems.push({
					textKey: "userSettingsMenuItemText",
					icon: "fad fa-user-cog",
					path: { name: "UserSettings" }
				});
			}
		}

		return menuItems;
	}
}
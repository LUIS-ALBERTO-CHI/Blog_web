import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";
import { CanAdministrateUsers } from '@/Fwamework/Users/users-permissions';
import { hasPermissionAsync } from "@/Fwamework/Permissions/Services/current-user-permissions-service";
import CustomFieldTypeService from "@UILibrary/Extensions/CellTemplate/Services/custom-field-type-service";
import CustomFieldAvatarType from "@/Fwamework/Users/Components/custom-field-avatar-type";
import { I18n } from "@/Fwamework/Culture/Services/localization-service";

export class UsersModule extends AbstractModule {
	async onInitAsync() {
		CustomFieldTypeService.register(CustomFieldAvatarType);

		const CurrentUserService = (await import("@/Fwamework/Users/Services/current-user-service")).default;
		await CurrentUserService.configureAsync();
	}

	async getMenuItemsAsync(menuType) {
		
		let menuItems = [];

		if (menuType === "administration" && await hasPermissionAsync(CanAdministrateUsers)) {
			menuItems.push(
				{
					groupIndex: 100,
					textKey: "usersMenuItemText",
					path: { name: "Users" },
					icon: "fas fa-user",
					descriptionKey: "usersMenuItemDescription",
					groupText: I18n.t('permissionsAndRightsGroupText'),
				});
		}
		return menuItems;
	}
}

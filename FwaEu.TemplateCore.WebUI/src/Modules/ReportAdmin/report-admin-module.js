import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";
import { CanAdministrateReports } from "@/Modules/ReportAdmin/report-admin-permissions";
import { hasPermissionAsync } from "@/Fwamework/Permissions/Services/current-user-permissions-service";

export class ReportAdminModule extends AbstractModule {
	onInitAsync() {
	}

	async getMenuItemsAsync(menuType) {
		let menuItems = [];

		if (menuType === "administration" && await hasPermissionAsync(CanAdministrateReports)) {
			menuItems.push({
				textKey: "reportAdminMenuItemText",
				groupKey: 'reportsGroupText',
				path: { name: "ReportAdmin" },
				icon: "fas fa-tools",
				descriptionKey: "reportAdminMenuItemDescription"
			});
		}
		return menuItems;
	}
}

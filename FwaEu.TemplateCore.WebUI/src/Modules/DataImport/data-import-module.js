import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";
import { CanImport } from "@/Modules/DataImport/data-import-permissions";
import { hasPermissionAsync } from "@/Fwamework/Permissions/Services/current-user-permissions-service";

export class DataImportModule extends AbstractModule {

    async getMenuItemsAsync(menuType) {
		let menuItems = [];

		if (menuType === "administration" && await hasPermissionAsync(CanImport)) {
            menuItems.push({
                textKey: "dataImportMenuItemText",
                path: { name: "Import" },
                icon: "fas fa-file-import",
                descriptionKey: "dataImportMenuItemDescription"
            });
        }
        return menuItems;
    }
}
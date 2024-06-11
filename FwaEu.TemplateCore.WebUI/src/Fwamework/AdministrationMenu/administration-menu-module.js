import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";
import AdministrationMenuService from "./Services/administration-menu-service";

export class AdministrationMenuModule extends AbstractModule {

    async getMenuItemsAsync(menuType) {

		let menuItems = [];
		if (menuType === "sideNavigation" && await AdministrationMenuService.anyMenuItemAvailableAsync()) {
			menuItems.push({
				visibleIndex: 2000,//NOTE: The administration must be the last item on the menu
                textKey: "administration",
				path: { name: "Administration" },
                icon: "fad fa-wrench",
				color:"#607d8b"
            });
        }
        return menuItems;
    }
}
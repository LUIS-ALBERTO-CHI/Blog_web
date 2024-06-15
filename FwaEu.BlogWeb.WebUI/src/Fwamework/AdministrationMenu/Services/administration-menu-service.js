import MenuService from '@/Fwamework/Menu/Services/menu-service';
import { I18n } from '@/Fwamework/Culture/Services/localization-service';

export const MenuType = 'administration';

class AdministrationMenuService extends MenuService {
	constructor() {
		super(MenuType);
	}

	localizeMenuItems(menuItems) {
		super.localizeMenuItems(menuItems);
		for (var menuItem of menuItems) {
			if (menuItem.groupKey) {
				menuItem.groupText = I18n.t(menuItem.groupKey);
			} else {
				menuItem.groupIndex = -1;
				menuItem.groupKey = "defaultGroupText";
				menuItem.groupText = I18n.t("defaultGroupText");
			}
		}
	}
}

export default new AdministrationMenuService();
import { HeaderItem } from "@/Modules/Header/Services/header-item";
import HeaderService from "@/Modules/Header/Services/header-service";
import PersistentNotificationsHeaderItemConfiguration from "./persistent-notifications-header-item-configuration";

export default {
	async configureAsync() {
		HeaderService.register(new HeaderItem(PersistentNotificationsHeaderItemConfiguration, true));
	}
}
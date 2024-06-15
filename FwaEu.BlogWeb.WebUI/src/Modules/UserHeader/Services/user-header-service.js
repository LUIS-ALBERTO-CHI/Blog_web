import { HeaderItem } from "@/Modules/Header/Services/header-item";
import HeaderService from "@/Modules/Header/Services/header-service";
import AuthenticationService from "@/Fwamework/Authentication/Services/authentication-service";
import UserHeaderItemConfiguration from "./user-header-item-configuration";

export default {
	async configureAsync() {

		const isHeaderVisible = await AuthenticationService.isAuthenticatedAsync();

		HeaderService.register(new HeaderItem(UserHeaderItemConfiguration, isHeaderVisible));

		AuthenticationService.onLoggedIn(() => HeaderService.setVisibility(UserHeaderItemConfiguration.key, true));
		AuthenticationService.onLoggedOut(() => HeaderService.setVisibility(UserHeaderItemConfiguration.key, false));
	}
}
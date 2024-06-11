import { HeaderItem } from "@/Modules/Header/Services/header-item";
import HeaderService from "@/Modules/Header/Services/header-service";
import SearchHeaderItemConfiguration from "./search-header-item-configuration";


export default {
	async configureAsync() {
		
		HeaderService.register(new HeaderItem(SearchHeaderItemConfiguration, true));
	}
}
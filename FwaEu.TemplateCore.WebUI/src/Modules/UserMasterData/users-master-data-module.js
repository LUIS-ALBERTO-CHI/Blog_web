import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";
import UsersMasterDataService from "./Services/users-master-data-service";

export class UsersMasterDataModule extends AbstractModule {
	async onInitAsync() {
		await UsersMasterDataService.configureAsync();
	}
}

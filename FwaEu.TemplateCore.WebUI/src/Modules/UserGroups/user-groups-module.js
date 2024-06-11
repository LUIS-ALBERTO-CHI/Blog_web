import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";
import UserGroupMasterDataService from "@/Modules/UserGroups/Services/user-groups-master-data-service"
import GenericAdminConfigurationService from '@/Modules/GenericAdmin/Services/generic-admin-configuration-service';
import UserGroupsConfiguration from '@/Modules/UserGroups/Services/user-groups-configuration';

export class UserGroupsModule extends AbstractModule {
	async onInitAsync() {
		await UserGroupMasterDataService.configureAsync();

		GenericAdminConfigurationService.register(UserGroupsConfiguration);
	}
}

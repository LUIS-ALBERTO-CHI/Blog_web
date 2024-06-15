import AbstractConfiguration from '@/Modules/GenericAdmin/Services/abstract-configuration';
import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
import { hasPermissionAsync } from '@/Fwamework/Permissions/Services/current-user-permissions-service';
import { CanAdministrateUserGroups } from '@/Modules/UserGroups/user-groups-permissions';

class UserGroupConfiguration extends AbstractConfiguration {
	constructor() {
		super();
	}

	async loadMessagesAsync(component) {
		await loadMessagesAsync(component, import.meta.glob('@/Modules/UserGroups/Content/user-groups-messages.*.json'));
    }
    
	getPageTitleKey() {
		return 'userGroupsTitle';
	}

	getGroupTextKey() {
		return 'permissionsAndRightsGroupText';
	}
}

export default {
	configurationKey: 'UserGroups',
	icon: "fas fa-users",
	getConfiguration: function () {
		return new UserGroupConfiguration();
	},
	async isAccessibleAsync() {
		return await hasPermissionAsync(CanAdministrateUserGroups);
	}
};
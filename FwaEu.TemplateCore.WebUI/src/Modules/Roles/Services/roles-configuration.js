import AbstractConfiguration from '@/Modules/GenericAdmin/Services/abstract-configuration';
import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
import { hasPermissionAsync } from '@/Fwamework/Permissions/Services/current-user-permissions-service';
import { CanAdministrateRoles } from '@/Modules/Roles/permissions-by-role-permissions';

class RoleConfiguration extends AbstractConfiguration {
	constructor() {
		super();
	}

	async loadMessagesAsync(component) {
		await loadMessagesAsync(component, import.meta.glob('@/Modules/Roles/Content/roles-messages.*.json'));
    }
    
	getPageTitleKey() {
		return 'roleTitle';
	}

	getGroupTextKey() {
		return 'permissionsAndRightsGroupText';
	}
}

export default {
	configurationKey: 'RoleEntity',
	icon: "fas fa-user-tag",
	getConfiguration: function () {
		return new RoleConfiguration();
	},
	async isAccessibleAsync() {
		return await hasPermissionAsync(CanAdministrateRoles);
	}
};
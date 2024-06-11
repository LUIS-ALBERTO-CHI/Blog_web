import AbstractConfiguration from '@/Modules/GenericAdmin/Services/abstract-configuration';
import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
import { CanAdministrateRoles } from '@/Modules/Roles/permissions-by-role-permissions';
import { hasPermissionAsync } from '@/Fwamework/Permissions/Services/current-user-permissions-service';

class RolePermissionConfiguration extends AbstractConfiguration {
	constructor() {
		super();
	}

	async loadMessagesAsync(component) {
		await loadMessagesAsync(component, import.meta.glob('@/Modules/Roles/Content/role-permission-common.*.json'));
    }
    
	getPageTitleKey() {
		return 'rolePermissionLinksTitle';
	}

	getDescriptionKey() {
		return 'rolePermissionLinksDescription';
	}

	getGroupTextKey() {
		return 'permissionsAndRightsGroupText';
	}
}

export default {
	configurationKey: 'RolePermissionEntity',
	icon: "fas fa-users-cog",
	getConfiguration: function () {
		return new RolePermissionConfiguration();
	},
	async isAccessibleAsync() {
		return await hasPermissionAsync(CanAdministrateRoles);
	}
};
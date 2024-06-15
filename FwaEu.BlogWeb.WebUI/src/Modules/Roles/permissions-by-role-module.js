import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";
import RolesMasterDataService from "@/Modules/Roles/Services/roles-master-data-service";
import RolePermissionsMasterDataService from "@/Modules/Roles/Services/role-permissions-master-data-service";
import CurrentUserPermissionsService from "@/Fwamework/Permissions/Services/current-user-permissions-service";
import CurrentUserPermissionsByRoleService from "@/Modules/Roles/Services/current-user-permissions-by-role-provider-service";
import GenericAdminConfigurationService from '@/Modules/GenericAdmin/Services/generic-admin-configuration-service';
import RolePermissionConfiguration from '@/Modules/Roles/Services/role-permission-configuration';
import RolesConfiguration from '@/Modules/Roles/Services/roles-configuration';

export class PermissionsByRoleModule extends AbstractModule {
	async onInitAsync() {
		await RolesMasterDataService.configureAsync();
		await RolePermissionsMasterDataService.configureAsync();

		GenericAdminConfigurationService.register(RolePermissionConfiguration);
		GenericAdminConfigurationService.register(RolesConfiguration);		
	}

	onApplicationCreated() {
		CurrentUserPermissionsService.setPermissionsProvider(CurrentUserPermissionsByRoleService);
	}
}

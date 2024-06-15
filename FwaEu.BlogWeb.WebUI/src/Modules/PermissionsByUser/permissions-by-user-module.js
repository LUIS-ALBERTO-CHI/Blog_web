import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";
import CurrentUserPermissionsByUserProviderService from "@/Modules/PermissionsByUser/Services/current-user-permissions-by-user-provider-service";
import CurrentUserPermissionsService from "@/Fwamework/Permissions/Services/current-user-permissions-service";

export class PermissionsByUserModule extends AbstractModule {

	onApplicationCreated() {
		CurrentUserPermissionsService.setPermissionsProvider(CurrentUserPermissionsByUserProviderService);
	}

}
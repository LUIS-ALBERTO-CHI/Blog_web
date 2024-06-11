import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";
import PermissionsMasterDataService from "@/Fwamework/Permissions/Services/permissions-master-data-service";
import PermissionsAuthorizationService from "@/Fwamework/Permissions/Services/http-permission-authorization-service";
import RoutingPermissionAuthorizationService from "@/Fwamework/Permissions/Services/routing-permission-authorization-service";


export class PermissionModule extends AbstractModule {
	async onInitAsync() {
		await PermissionsMasterDataService.configureAsync();
		PermissionsAuthorizationService.configure();
		RoutingPermissionAuthorizationService.configure();
	}
}

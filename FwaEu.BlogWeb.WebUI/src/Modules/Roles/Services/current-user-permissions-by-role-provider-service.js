import CurrentUserService from "@/Fwamework/Users/Services/current-user-service";
import RolesPermissionsMasterDataService from "@/Modules/Roles/Services/role-permissions-master-data-service";
import PermissionsMasterDataService from "@/Fwamework/Permissions/Services/permissions-master-data-service";

export default {
	getCurrentUserPermissionsAsync: async function () {
		let currentUser = await CurrentUserService.getAsync();

		if (currentUser?.parts?.adminState?.isAdmin) {
			let permissions = await PermissionsMasterDataService.getAllAsync();
			return permissions.map(md => md.invariantId);
		}

		let rolesMasterData = await RolesPermissionsMasterDataService.getAllAsync();
		let result =  currentUser?.parts?.roles?.selectedIds
			.flatMap(roleId => rolesMasterData.filter(md => md.roleId == roleId)
				.map(md => md.permissionInvariantId)) ?? [];

		return result;
	}
};
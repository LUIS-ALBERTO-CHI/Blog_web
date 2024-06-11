import { I18n } from "@/Fwamework/Culture/Services/localization-service";
import MasterDataService from "./permissions-master-data-service";
let permissionsProvider = null;

export const CheckOperation = { All: 'all', Any: 'any' };

export const hasPermissionAsync = async (permission) => await hasAnyPermissionAsync([permission]);
export const hasAnyPermissionAsync = async (permissions) => await hasPermissionsAsync(permissions, CheckOperation.Any);
export const hasAllPermissionsAsync = async (permissions) => await hasPermissionsAsync(permissions, CheckOperation.All);

export const requirePermissionAsync = async (permission) => {
	const hasPermission = await hasPermissionAsync(permission);
	if (!hasPermission) {
		throw new Error(await getAccessDeniedForUserByPermissionMessageAsync([permission]));
	}
};

export const requireAnyPermissionAsync = async (permissions) => {
	const hasPermissions = await hasAnyPermissionAsync(permissions);
	if (!hasPermissions) {
		throw new Error(await getAccessDeniedForUserByAnyPermissionMessageAsync(permissions));
	}
};

export const requireAllPermissionsAsync = async (permissions) => {
	const hasPermissions = await hasAllPermissionsAsync(permissions);
	if (!hasPermissions) {
		throw new Error(await getAccessDeniedForUserByAllPermissionsMessageAsync(permissions));
	}
};

export const getAccessDeniedForUserByPermissionMessageAsync = async (permissions) => await getPermissionsMessageAsync("accessDeniedForUserByPermission", permissions);
export const getAccessDeniedForUserByAnyPermissionMessageAsync = async (permissions) => await getPermissionsMessageAsync("accessDeniedForUserByAnyPermission", permissions);
export const getAccessDeniedForUserByAllPermissionsMessageAsync = async (permissions) => await getPermissionsMessageAsync("accessDeniedForUserByAllPermissions", permissions);

export default {
	hasPermissionAsync,
	hasAnyPermissionAsync,
	hasAllPermissionsAsync,

	requirePermissionAsync,
	requireAnyPermissionAsync,
	requireAllPermissionsAsync,

	getAccessDeniedForUserByPermissionMessageAsync,
	getAccessDeniedForUserByAnyPermissionMessageAsync,
	getAccessDeniedForUserByAllPermissionsMessageAsync,

	setPermissionsProvider(currentUserPermissionsProvider) {
		if (!currentUserPermissionsProvider.getCurrentUserPermissionsAsync) {
			throw new Error("currentUserPermissionsProvider must have a 'getCurrentUserPermissionsAsync' function.")
		}
		permissionsProvider = currentUserPermissionsProvider;
	}
}

/**
	 * Only for internal module pruposes, prefer to use hasAllPermissionsAsync and hasAnyPermissionAsync functions
	 * @param {Array} permissions
	 * @param {'all' | 'any'} operation 
	 */
export const hasPermissionsAsync = async function (permissions, operation) {

	if (operation !== CheckOperation.Any && operation !== CheckOperation.All)
		throw new Error(`Invalid operation for permissions check (${operation}), available operations are '${CheckOperation.All}' and '${CheckOperation.Any}'`);

	const authenticationService = (await import("@/Fwamework/Authentication/Services/authentication-service")).default;
	const currentUserService = (await import("@/Fwamework/Users/Services/current-user-service")).default;

	const isCurrentUserAuthenticated = await authenticationService.isAuthenticatedAsync();
	if (!isCurrentUserAuthenticated)
		return false;

	let currentUser = await currentUserService.getAsync();
	if (currentUser?.parts?.adminState?.isAdmin) {
		return true;
	}
	let currentUserPermissionsIds = await permissionsProvider.getCurrentUserPermissionsAsync(); // returns a list of permisions id
	let grantedPermissions = permissions.filter(perm => currentUserPermissionsIds.includes(perm));
	return operation === CheckOperation.Any ? grantedPermissions.length > 0 : grantedPermissions.length === permissions.length;
};

const getPermissionsMessageAsync = async function (messageKey, permissions) {
	const permissionsMasterData = await MasterDataService.getByIdsAsync(permissions);
	return I18n.t(messageKey, [permissionsMasterData.join(", ")]);
};
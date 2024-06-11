import { SearchProvider } from "@/Modules/Search/Services/search-provider";
import { SearchResultItem } from "@/Modules/Search/Services/search-result-item";
import UserFormatterService from "@/Fwamework/Users/Services/user-formatter-service";
import { I18n } from "@/Fwamework/Culture/Services/localization-service";
import { hasPermissionAsync } from "@/Fwamework/Permissions/Services/current-user-permissions-service";
import { CanAdministrateUsers } from "@/Fwamework/Users/users-permissions";

const processUserResultsAsync = async function (results) {
	return results.map(r =>
		new SearchResultItem(
			'fas fa-user',
			`#${r.id} ${UserFormatterService.getUserFullName(r)}`,
			r.identity,
			{ name: 'EditUserDetails', params: { id: r.id } })
	);
};

export class UserSearchProvider extends SearchProvider {
	icon = 'fas fa-user';
	key = 'User';
	displayName = I18n.t('usersFilterDisplayName');
	isAvailableAsync = async function() {
		return await hasPermissionAsync(CanAdministrateUsers);
	}
	processResultsAsync = processUserResultsAsync
}
export class UserIdSearchProvider extends SearchProvider {
	icon = 'fas fa-user-tag';
	key = 'UserId';
	displayName = I18n.t('userIdsFilterDisplayName');
	isAvailableAsync = async function () {
		return await hasPermissionAsync(CanAdministrateUsers);
	}
	processResultsAsync = processUserResultsAsync
}
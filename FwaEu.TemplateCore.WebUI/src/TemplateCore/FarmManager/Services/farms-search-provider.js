import { SearchProvider } from "@/Modules/Search/Services/search-provider";
import { SearchResultItem } from "@/Modules/Search/Services/search-result-item";
import { I18n } from "@/Fwamework/Culture/Services/localization-service";
import { hasPermissionAsync } from "@/Fwamework/Permissions/Services/current-user-permissions-service";
import { CanAccessToFarmManager } from "../farms-permissions";
import { CanAdministrateUsers } from "@/Fwamework/Users/users-permissions";

const processFarmResultsAsync = async function (results) {
	return results.map(r =>
		new SearchResultItem(
			'fas fa-farm',
			`#${r.id} ${r.name}`,
			r.identity,
			{ name: 'FarmSummary', params: { id: r.id } })
	);
};

export class FarmSearchProvider extends SearchProvider {
	icon = 'fas fa-farm';
	key = 'Farm';
	displayName = I18n.t('farmsFilterDisplayName');
	isAvailableAsync = async function() {
		return await hasPermissionAsync(CanAccessToFarmManager);
	}
	processResultsAsync = processFarmResultsAsync
}
export class FarmIdSearchProvider extends SearchProvider {
	icon = 'fas fa-farm';
	key = 'FarmId';
	displayName = I18n.t('farmIdsFilterDisplayName');
	isAvailableAsync = async function () {
		return await hasPermissionAsync(CanAccessToFarmManager);
	}
	processResultsAsync = processFarmResultsAsync
}

export class FarmerSearchProvider extends SearchProvider {
	icon = 'fas fa-user-cowboy';
	key = 'Farmer';
	displayName = I18n.t('farmerFilterDisplayName');
	isAvailableAsync = async function () {
		return await hasPermissionAsync(CanAdministrateUsers);
	}
	processResultsAsync = async function (results) {
		return results.map(r =>
			new SearchResultItem(
				this.icon,
				`#${r.id} ${r.farmerPseudonym}`,
				r.identity,
				{ name: 'EditUserDetails', params: { id: r.id } })
		);
	}
}
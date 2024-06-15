import AbstractConfiguration from '@/Modules/GenericAdmin/Services/abstract-configuration';
import { hasPermissionAsync } from '@/Fwamework/Permissions/Services/current-user-permissions-service';
import { CanAdministrateFarmMasterData } from '@/BlogWeb/FarmManager/farms-permissions';

class FarmAnimalSpeciesConfiguration extends AbstractConfiguration {
	constructor() {
		super();
	}
    
	getPageTitleKey() {
		return 'farmAnimalSpeciesTitle';
	}

	getGroupTextKey() {
		return 'farmManagerGroupText';
	}
}

export default {
	configurationKey: 'FarmAnimalSpecies',
	icon: "fas fa-pig",
	getConfiguration: function () {
		return new FarmAnimalSpeciesConfiguration();
	},
	async isAccessibleAsync() {
		return await hasPermissionAsync(CanAdministrateFarmMasterData);
	}
};
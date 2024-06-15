import AbstractConfiguration from '@/Modules/GenericAdmin/Services/abstract-configuration';
import { hasPermissionAsync } from '@/Fwamework/Permissions/Services/current-user-permissions-service';
import { CanAdministrateFarmMasterData } from '@/BlogWeb/FarmManager/farms-permissions';

class FarmActivityConfiguration extends AbstractConfiguration {
	constructor() {
		super();
	}
    
	getPageTitleKey() {
		return 'farmActivitiesTitle';
	}

	getGroupTextKey() {
		return 'farmManagerGroupText';
	}
}

export default {
	configurationKey: 'FarmActivities',
	icon: "fas fa-tractor",
	getConfiguration: function () {
		return new FarmActivityConfiguration();
	},
	async isAccessibleAsync() {
		return await hasPermissionAsync(CanAdministrateFarmMasterData);
	}
};
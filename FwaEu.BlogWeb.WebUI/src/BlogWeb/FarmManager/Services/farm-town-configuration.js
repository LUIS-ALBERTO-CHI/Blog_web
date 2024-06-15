import AbstractConfiguration from '@/Modules/GenericAdmin/Services/abstract-configuration';
import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
import { hasPermissionAsync } from '@/Fwamework/Permissions/Services/current-user-permissions-service';
import { CanAdministrateFarmMasterData } from '@/BlogWeb/FarmManager/farms-permissions';

class FarmTownConfiguration extends AbstractConfiguration {
	constructor() {
		super(true);
	}

	async loadMessagesAsync(component) {
		await loadMessagesAsync(component, import.meta.glob('@/BlogWeb/FarmManager/Content/farm-town-messages.*.json'));
    }

	getPageTitleKey() {
		return 'farmTownsTitle';
	}

	getGroupTextKey() {
		return 'farmManagerGroupText';
	}
	async onColumnsCreatingAsync(component, columns) {
		await super.onColumnsCreatingAsync(component, columns);
		//TODO: Issues when creating new values because required culture client side is different from required culture server side https://dev.azure.com/fwaeu/BlogWeb/_workitems/edit/6961 
	}
}

export default {
	configurationKey: 'FarmTowns',
	icon: "fas fa-house-day",
	getConfiguration: function () {
		return new FarmTownConfiguration();
	},
	async isAccessibleAsync() {
		return await hasPermissionAsync(CanAdministrateFarmMasterData);
	}
};
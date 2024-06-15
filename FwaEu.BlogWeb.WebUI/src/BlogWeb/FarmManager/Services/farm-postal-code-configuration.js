import AbstractConfiguration from '@/Modules/GenericAdmin/Services/abstract-configuration';
import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
import { hasPermissionAsync } from '@/Fwamework/Permissions/Services/current-user-permissions-service';
import { CanAdministrateFarmMasterData } from '@/BlogWeb/FarmManager/farms-permissions';

import DataGridDataSourceFactory from '@UILibrary/Modules/MasterData/Services/data-grid-data-source-factory';

class FarmTownPostalCodeConfiguration extends AbstractConfiguration {
	constructor() {
		super(true);
	}

	async loadMessagesAsync(component) {
		await loadMessagesAsync(component, import.meta.glob('@/BlogWeb/FarmManager/Content/farm-postal-code-messages.*.json'));
    }
        
	getPageTitleKey() {
		return 'farmPostalCodeTitle';
	}

	getGroupTextKey() {
		return 'farmManagerGroupText';
	}

	createDataSource(component, getDataGridRef) {
		return DataGridDataSourceFactory.createDataSource({
			getDataGrid: getDataGridRef,
			storeOptions: this.createDataSourceOptions(component)
		});
	}
}

export default {
	configurationKey: 'FarmPostalCodes',
	icon: "fas fa-address-card",
	getConfiguration: function () {
		return new FarmTownPostalCodeConfiguration();
	},
	async isAccessibleAsync() {
		return await hasPermissionAsync(CanAdministrateFarmMasterData);
	}
};
import AbstractConfiguration from '@/Modules/GenericAdmin/Services/abstract-configuration';
import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
import { CanAdministrateReports } from '@/Modules/ReportAdmin/report-admin-permissions';
import { hasPermissionAsync } from '@/Fwamework/Permissions/Services/current-user-permissions-service';

class ReportCategoryConfiguration extends AbstractConfiguration {
	constructor() {
		super();
	}

	async loadMessagesAsync(component) {
		await loadMessagesAsync(component, import.meta.glob('@/Modules/ReportMasterData/Content/report-category-messages.*.json'));
    }
    
	getPageTitleKey() {
		return 'reportCategoriesTitle';
	}

	getDescriptionKey() {
		return 'reportCategoriesDescription';
	}

	getGroupTextKey() {
		return 'reportsGroupText';
	}
}

export default {
	configurationKey: 'ReportCategoryEntity',
	icon: "fas fa-list-ul",
	getConfiguration: function () {
		return new ReportCategoryConfiguration();
	},
	async isAccessibleAsync() {
		return await hasPermissionAsync(CanAdministrateReports);
	}
};
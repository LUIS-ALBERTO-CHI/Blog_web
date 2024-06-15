import AbstractConfiguration from '@/Modules/GenericAdmin/Services/abstract-configuration';
import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
import { CanAdministrateReports } from '@/Modules/ReportAdmin/report-admin-permissions';
import { hasPermissionAsync } from '@/Fwamework/Permissions/Services/current-user-permissions-service';
import ReportsFilterServiceService from '@/Modules/Reports/Services/reports-filters-service';
import ReportDataSourceService from "@/Modules/Reports/Services/report-data-source-service";

class ReportFieldConfiguration extends AbstractConfiguration {
	constructor() {
		super();
	}

	async loadMessagesAsync(component) {
		await loadMessagesAsync(component, import.meta.glob('@/Modules/ReportMasterData/Content/report-field-messages.*.json'));
    }
    
	getPageTitleKey() {
		return 'reportFieldsTitle';
	}

	getDescriptionKey() {
		return 'reportFieldsDescription';
	}

	getGroupTextKey() {
		return 'reportsGroupText';
	}

	async onColumnsCreatingAsync(component, columns) {
		await super.onColumnsCreatingAsync(component, columns);
		columns.find(c => c.dataField == "dotNetTypeName").lookup = {
			dataSource: ReportsFilterServiceService.getParameterTypes(),
			valueExpr: "value",
			displayExpr: "key",
			showClearButton: true,
		};
		columns.find(c => c.dataField == "dataSourceType").lookup = {
			dataSource: ReportDataSourceService.getAllDataSourceTypesOrderedForDropdown(),
			valueExpr: "type",
			displayExpr: "type",
			showClearButton: true,
		};

		//TODO:charger le bon component pour le champ dataSourceArgument https://fwaeu.visualstudio.com/BlogWeb/_workitems/edit/7351
		// En attendant on le change en TextArea
		columns.find(c => c.dataField == "dataSourceArgument").formItem.editorType = 'dxTextArea';
	}

}

export default {
	configurationKey: 'ReportFieldEntity',
	icon: "fas fa-toolbox",
	getConfiguration: function () {
		return new ReportFieldConfiguration();
	},
	async isAccessibleAsync() {
		return await hasPermissionAsync(CanAdministrateReports);
	}
};
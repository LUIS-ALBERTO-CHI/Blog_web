import ReportFilterDataSourceFactory from "@/Modules/ReportAdmin/Services/report-filter-data-source-factory.js";
import { I18n } from "@/Fwamework/Culture/Services/localization-service";


export default {
	getPrefilterMenuItems(componentVm) {
		return [{
			icon: 'showpanel',
			text: I18n.t('manageFilters'),
			action: () => componentVm.$router.push({ name: 'GenericAdmin/ReportFilterEntity' }),
		},
		{
			icon: 'refresh',
			text: I18n.t('refreshFilters'),
			action: () => {
				componentVm.reportFilterCodesDataSource.dataSource = ReportFilterDataSourceFactory.createSelectBoxDataSource();
				componentVm.$refs['dataAndFiltersGrid'].instance.refresh();
			}
		}]
	}
}
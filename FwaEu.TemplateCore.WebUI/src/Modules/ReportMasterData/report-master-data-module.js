import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";
import ReportDataSourceService from "@/Modules/Reports/Services/report-data-source-service";
import ReportMasterDataSourceHandler from "@/Modules/ReportMasterData/Services/report-master-data-source-handler";
import ReportFieldMasterDataService from "./Services/report-field-master-data-service";
import ReportFilterMasterDataService from "./Services/report-filter-master-data-service";
import ReportCategoryMasterDataService from "./Services/report-category-master-data-service";
import ReportSummaryTypeMasterDataService from "./Services/report-summary-type-master-data-service";
import GenericAdminConfigurationService from '@/Modules/GenericAdmin/Services/generic-admin-configuration-service';
import ReportFieldConfiguration from '@/Modules/ReportMasterData/Services/report-field-configuration';
import ReportCategoryConfiguration from '@/Modules/ReportMasterData/Services/report-category-configuration';
import ReportFilterConfiguration from '@/Modules/ReportMasterData/Services/report-filter-configuration';

export class ReportMasterDataModule extends AbstractModule {
	async onInitAsync() {
		ReportDataSourceService.addDataSourceType(ReportMasterDataSourceHandler);
		
		await ReportFieldMasterDataService.configureAsync();
		await ReportFilterMasterDataService.configureAsync();
		await ReportCategoryMasterDataService.configureAsync();
		await ReportSummaryTypeMasterDataService.configureAsync();

		GenericAdminConfigurationService.register(ReportFieldConfiguration);
		GenericAdminConfigurationService.register(ReportCategoryConfiguration);
		GenericAdminConfigurationService.register(ReportFilterConfiguration);
	}
}

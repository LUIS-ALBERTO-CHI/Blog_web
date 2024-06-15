import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";
import FarmDatasDataSourceHandler from "./Services/farm-datas-data-source-handler"; 
import FarmInfosDataSourceHandler from "./Services/farm-infos-data-source-handler";
import ReportDataSourceService from "@/Modules/Reports/Services/report-data-source-service"; 

export class FarmManagerReportModule extends AbstractModule {
	onInitAsync() {
		ReportDataSourceService.addDataSourceType(FarmDatasDataSourceHandler);
		ReportDataSourceService.addDataSourceType(new FarmInfosDataSourceHandler());
	}
}


import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";
import ReportDataSourceService from "@/Modules/Reports/Services/report-data-source-service";
import ReportSqlDataSourceHandler from "../ReportServerData/Services/report-sql-data-source-handler";

export class ReportServerDataModule extends AbstractModule {
	onInitAsync() {
		ReportDataSourceService.addDataSourceType(new ReportSqlDataSourceHandler());
	}
}

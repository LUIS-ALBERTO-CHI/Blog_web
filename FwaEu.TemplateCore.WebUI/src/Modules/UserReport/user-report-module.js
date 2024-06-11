import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";
import ReportDataService from "@/Modules/Reports/Services/report-data-service";
import UserStateCustomDatasourceProvider from "./Services/user-state-custom-datasource-provider";

export class UserReportModule extends AbstractModule {
	onInitAsync() {
		ReportDataService.addDataHandler(UserStateCustomDatasourceProvider);
	}
}
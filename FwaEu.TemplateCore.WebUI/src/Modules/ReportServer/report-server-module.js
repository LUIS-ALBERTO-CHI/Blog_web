import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";
import ReportsService from "@/Modules/Reports/Services/reports-service";
import ReportServerProvider from "@/Modules/ReportServer/Services/report-server-provider";

export class ReportServerModule extends AbstractModule {
	onInitAsync() {
		ReportsService.addReportProvider(ReportServerProvider);
	}
}

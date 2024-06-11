import ReportService from "@/Modules/Reports/Services/reports-service"
import { CanViewReports } from "@/Modules/Reports/reports-permissions";
import { hasPermissionAsync } from "@/Fwamework/Permissions/Services/current-user-permissions-service";

export default {
	titleKey: 'userReportsTitle',
	key: 'Reports',
	async isPerimeterAccessibleAsync() {
		return await hasPermissionAsync(CanViewReports);
	},
	async fetchDataAsync() {
		return await (await ReportService.getAllAsync())
			.map(ug => {
				return { id: ug.model.invariantId, name: ug.model.name };
			}
			);
	}
}
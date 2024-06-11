import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";
import ReportsService from "@/Modules/Reports/Services/reports-service";
import { CanViewReports } from "@/Modules/Reports/reports-permissions";
import { hasPermissionAsync } from "@/Fwamework/Permissions/Services/current-user-permissions-service";

export class ReportsModule extends AbstractModule {
	onInitAsync() {
	}
	async getMenuItemsAsync(menuType) {

		let menuItems = [];
		if (menuType === "sideNavigation" && await hasPermissionAsync(CanViewReports)) {
			let reports = await ReportsService.getAllAsync();
			let reportSubMenuItems = [];
			let anyMenuItemAccesible = false;
			let anySummaryItemAccesible = false;

			for (let report of reports) {
				let isReportMenuAccesible = report.model.navigation.menu?.visible ?? false;
				let isReportSummaryAccesible = report.model.navigation.summary?.visible ?? false;

				anyMenuItemAccesible |= isReportMenuAccesible;
				anySummaryItemAccesible |= isReportSummaryAccesible;
				if (isReportMenuAccesible) {
					
					let item =
					{
						text: report.model.name,
						path: report.route,
						icon: report.model.icon ?? this.defaultReportIcon,
						color:"#b31958"
					};
					reportSubMenuItems.push(item);
				}
			}
			if (anyMenuItemAccesible || anySummaryItemAccesible) {
				let reportItem = {
					textKey: "reports",
					icon: "fad fa-file-chart-line",
					color:"#b31958"
				};
				if (!anyMenuItemAccesible) {
					reportItem.path = { name: "Reports" };
				}
				else if (anySummaryItemAccesible) {
					reportSubMenuItems.push({
						textKey: "allReports",
						path: { name: "Reports" },
						icon: this.defaultReportIcon
					});
				}
				reportItem.items = reportSubMenuItems;

				menuItems.push(reportItem);
			}
		}
		return menuItems;
	}

}

	export const DefaultReportIcon = "fal fa-file-chart-line";
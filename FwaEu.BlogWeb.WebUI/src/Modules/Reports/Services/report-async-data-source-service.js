import HttpService from '@/Fwamework/Core/Services/http-service';
import NotificationService from "@/Fwamework/Notifications/Services/notification-service";
import { Configuration } from "@/Fwamework/Core/Services/configuration-service";
import { I18n } from '@/Fwamework/Culture/Services/localization-service';
import { createRouterLink } from '@/Fwamework/Routing/Services/router-link-helper';


export default {
	async getQueueGuidAsync(invariantId, filters) {
		let response = await HttpService.post(`Reports/Async/Enqueue/${invariantId}`, { filters });
		return response.data;
	},
	async getStateAsync(queueGuid) {
		let response = await HttpService.get(`Reports/Async/State/${queueGuid}`);
		return response.data;
	},
	async getDataReportAsync(reportCacheStoreKey) {
		let response = await HttpService.get(`Reports/Async/Data/${reportCacheStoreKey}`);
		return response.data.rows;
	},

	async queueDataSourceReportAsync(report, filters) {
		let result = await this.getQueueGuidAsync(report.invariantId, filters)
		const loadContext = { report, queueGuid: result.queueGuid, intervalId: null };
		loadContext.intervalId = setInterval(function () {
			getState(loadContext);
		},
			Configuration.reports.asyncDataPoolingTimeInSeconds * 1000);

		async function getState(loadContext) {
			let stateResult = await HttpService.get(`Reports/Async/State/${loadContext.queueGuid}`);
			if (stateResult.data.state === "Finished") {
				clearInterval(loadContext.intervalId);
				const reportRoute = {
					name: "Report",
					params: { invariantId: loadContext.report.invariantId },
					query: { reportCacheStoreKey: stateResult.data.reportCacheStoreKey }
				};
				if (Object.keys(loadContext.report.filters).length > 0) {
					reportRoute.query.filters = JSON.stringify(loadContext.report.filters);
				}
				const componentInstance = createRouterLink({ to: reportRoute }, I18n.t('linkReportAsync'));

				NotificationService.showInformation(I18n.t('textLinkReportAsync', { reportName: report.name, link: componentInstance.$el.outerHTML }), { timeout: false });

				componentInstance.unmount();
			}
		}
	},


}
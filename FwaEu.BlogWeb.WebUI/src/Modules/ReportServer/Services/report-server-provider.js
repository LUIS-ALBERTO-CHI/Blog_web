import HttpService from '@/Fwamework/Core/Services/http-service';

export default {
	async getAllAsync() {
		const reports = (await HttpService.post('Reports', {})).data;
		return reports.map(reportModel => {
			return {
				model: reportModel,
				route: {
					name: reportModel.hasFilters ? "ReportFilter" : "Report",
					params: { invariantId: reportModel.invariantId }
				}
			};
		});
	},
	async getByInvariantIdAsync(invariantId) {
		const reportModel = (await HttpService.post(`Reports/${invariantId}`, {})).data;
		return reportModel;
	}
}


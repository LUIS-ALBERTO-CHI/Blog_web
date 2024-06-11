import HttpService from '@/Fwamework/Core/Services/http-service';

class ReportServerDataSourceHandlerBase {
	async getDataSourceAsync(invariantId, dataSource, filters, viewContext) {
		let response = await HttpService.post(`/Reports/${invariantId}/Data`, { filters: filters, argument: dataSource.argument });
		return response.data.rows;
	}
	async getDataSourceForAdminAsync(reportModel, loadParameters) {
		const loadDataSource = {
			type: reportModel.dataSource.type,
			argument: reportModel.dataSource.argument,
			parameters: loadParameters,
		};
		const result = await HttpService.post(`Reports/Admin/Data`, loadDataSource);
		return result.data.rows;
	}
	async getFilterDataSourceAsync(invariantId, dataSource, filters, viewContext) {
		let response = await HttpService.post(`/Reports/Filters/Data/${invariantId}/Data`, { filters: filters, argument: dataSource.argument });
		return response.data.rows;
	}
	async getFieldDataSourceAsync(invariantId, dataSource, filters, viewContext) {
		let response = await HttpService.post(`/Reports/Fields/Data/${invariantId}/Data`, { filters: filters, argument: dataSource.argument });
		return response.data.rows;
	}
}

export default ReportServerDataSourceHandlerBase;


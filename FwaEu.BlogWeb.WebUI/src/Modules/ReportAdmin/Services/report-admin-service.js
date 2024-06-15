import HttpService from '@/Fwamework/Core/Services/http-service';
import { LocalStorage } from "@/Fwamework/Storage/Services/local-storage-store";
import ReportDataSourceService from "@/Modules/Reports/Services/report-data-source-service";
import ReportFilterMasterDataService from "@/Modules/ReportMasterData/Services/report-filter-master-data-service";

export default {
	async getAllAsync() {
		let response = await HttpService.get(`/Reports/Admin`);
		return response.data;
	},
	async getReportAdminByInvariantIdAsync(invariantId) {
		if (!invariantId) {
			throw 'Invariant Id is required to get a report.';
		}

		let response = await HttpService.get(`/Reports/Admin/${invariantId}`);
		return response.data;
	},
	async saveViewsAsync(reportModel) {
		let result = await HttpService.post(`Reports/Views`, reportModel);
		return result.data;
	},
	async getReportDataSourcePropertiesAsync(reportModel, loadParameters) {
		const data = await this.getReportDataForAdminAsync(reportModel, loadParameters);
		return data && data.length > 0 ?
			Object.getOwnPropertyNames(data[0]).map(x => this.capitalize(x)) :
			[];
	},
	async getReportDataForAdminAsync(reportModel, loadParameters) {
		var dataSourceType = ReportDataSourceService.get(reportModel.dataSource.type);
		if (!dataSourceType)
			throw new Error(`Unsupported data source type: ${reportModel.dataSource.type}`);

		let data;
		// NOTE:Specific server side call for admin
		if (typeof (dataSourceType.getDataSourceForAdminAsync) === "function") {
			data = await dataSourceType.getDataSourceForAdminAsync(reportModel, loadParameters);
		}
		else
			data = await dataSourceType.getDataSourceAsync(reportModel.invariantId, reportModel.dataSource, loadParameters, {});

		return data;
	},

	async deleteAsync(invariantId) {
		await HttpService.delete(`Reports/Admin/${invariantId}`);
	},

	capitalize(value) {
		if (!value || value.length == 0) return value;
		return value[0].toUpperCase() + value.slice(1);
	},
	createEmptyViewAdminTab(viewType, index) {
		return {
			viewType: viewType,
			id: index,
			isNew: true,
			refName: `reportAdminViewTabs_${viewType}_${index}`,
			view: {
				isDefault: false,
				name: {
					en: '',
					fr: '',
				},
				value: '',
			},
		};
	},

	async createAdminLoadDataSourceParametersFromDataSourceAsync(report) {
		if (report?.dataSource?.argument == null)
			return [];

		var dataSourceType = ReportDataSourceService.get(report.dataSource.type);
		if (!dataSourceType || !dataSourceType.useCustomParameters)
			return [];

		const reportFilters = await ReportFilterMasterDataService.getAllAsync();
		const createLoadParameterFunction = this.createLoadParameter;
		let loadDataSourceParameters = report.filters.map(function (filter) {
			const dotNetTypeName = reportFilters.find(f => f.invariantId == filter.invariantId).dotNetTypeName;
			return createLoadParameterFunction(report.invariantId, filter.invariantId, "Filter", dotNetTypeName);
		});
		const currentFiltersInvariantIds = report.filters.map(f => f.invariantId);
		const findParametersRegex = /\:([\w.$]+|"[^"]+"|'[^']+')/g;
		const parameters = report.dataSource.argument.match(findParametersRegex);
		if (parameters) {
			parameters.map(p => p.substring(1))
				.filter(p => currentFiltersInvariantIds.indexOf(p) == -1)
				.forEach(function (parameter) {
					loadDataSourceParameters.push(createLoadParameterFunction(report.invariantId, parameter))
				});
		}

		return loadDataSourceParameters;
	},

	createLoadParameter(reportInvariantId, invariantId, dataSource, dotNetTypeName) {
		const parameter = LocalStorage.getValue(`${reportInvariantId}_${invariantId}`) ??
			{ dotNetTypeName: dotNetTypeName ?? "String", value: null, }
		return {
			invariantId: invariantId,
			dataSource: dataSource,
			dotNetTypeName: parameter.dotNetTypeName,
			value: parameter.value,
		}
	},

	saveParameterValuesInLocalStorage(reportInvariantId, parameters) {
		if (parameters && parameters.length) {
			parameters.forEach(function (parameter) {
				LocalStorage.setValue(`${reportInvariantId}_${parameter.invariantId}`,
					{ dotNetTypeName: parameter.dotNetTypeName, value: parameter.value });
			});
		}
	},
	async saveAsync(invariantId, model) {
		let result = await HttpService.post(`Reports/Admin/${invariantId}`, model);
		return result.data;
	},
	async downloadRawJsonAsync(invariantId) {
		let result = await HttpService.get(`Reports/Admin/${invariantId}/RawJson`);
		const blobFile = new Blob([JSON.stringify(result.data)]);
		HttpService.saveBlobFile(blobFile, false, `${invariantId}.report.json`);
	},
}
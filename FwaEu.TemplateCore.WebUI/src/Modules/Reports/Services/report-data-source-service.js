import ReportDataService from "@/Modules/Reports/Services/report-data-service";

let reportDataSourceTypes = [];

export default {
	addDataSourceType(type) {
		if (!type.type || typeof (type.getDataSourceAsync) !== 'function') {
			throw new Error("The handler must implement a 'type' property");
		}
		reportDataSourceTypes.push(type);
		ReportDataService.addDataHandler({
			type: type.type,
			getDataSourceAsync: type.getDataSourceAsync,
			//NOTE:use getDataSourceAsync when getFilterDataSourceAsync or getFieldDataSourceAsync aren't overriden
			getFilterDataSourceAsync: type.getFilterDataSourceAsync ?? type.getDataSourceAsync, 
			getFieldDataSourceAsync: type.getFieldDataSourceAsync ?? type.getDataSourceAsync,
		});
	},
	getAll() {
		return reportDataSourceTypes;
	},
	get(dataSourceType) {
		return reportDataSourceTypes.find(data => data.type === dataSourceType);
	},
	getAllDataSourceTypesOrderedForDropdown() {
		return this.getAll().sort((a, b) => (a.displayOrder ?? 0) - (b.displayOrder ?? 0))
			.map(function (dataSourceType) {
				return {
					type: dataSourceType.type,
					icon: dataSourceType.icon,
				}
			});
	},
	getNecessaryHandler(dataSourceType) {
		const handler = ReportDataService.get(dataSourceType);
		if (!handler) {
			throw new Error(`Unsupported data source type: ${dataSourceType}`);
		}
		return handler;
	},
	async getDataSourceAsync(invariantId, dataSource, filters, viewContext) {
		return await this.getNecessaryHandler(dataSource.type)
			.getDataSourceAsync(invariantId, dataSource, filters, viewContext);
	},
	async getFilterDataSourceAsync(invariantId, dataSource, filters, viewContext) {
		return await this.getNecessaryHandler(dataSource.type)
			.getFilterDataSourceAsync(invariantId, dataSource, filters, viewContext);
	},
	async getFieldDataSourceAsync(invariantId, dataSource, filters, viewContext) {
		return await this.getNecessaryHandler(dataSource.type)
			.getFieldDataSourceAsync(invariantId, dataSource, filters, viewContext);
	}
}
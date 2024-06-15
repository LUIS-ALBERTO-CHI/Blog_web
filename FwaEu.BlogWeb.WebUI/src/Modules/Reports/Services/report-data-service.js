let reportDataHandlers = [];

export default {
	addDataHandler(handler) {
		if (!handler.type || typeof (handler.getDataSourceAsync) !== 'function') {
			throw new Error("The handler must implement a 'type' property and 'getDataSourceAsync' function");
		}
		reportDataHandlers.push(handler);
	},
	get(dataSourceType) {
		return reportDataHandlers.find(data => data.type === dataSourceType);
	},
}
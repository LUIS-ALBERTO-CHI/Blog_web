
let providers = [];

export default {

	addReportProvider(provider) {
		if (typeof (provider.getAllAsync) !== 'function' || typeof (provider.getByInvariantIdAsync) !== 'function') {

			throw "The provider must implement getAllAsync and getByInvariantIdAsync functions";
		}
		providers.push(provider);
	},
/**
 * @typedef { } ReportModel
 * @typedef { {model: ReportModel, route: import("vue-router").RawLocation}} ReportListItem
 * 
 * @returns { Promise<Array<ReportListItem>>}
 * */
	async getAllAsync() {
		let reports = (await Promise.all(providers.map(x => x.getAllAsync()))).flat();
		return reports;
	},

	/**
	 * 
	 * @param {String} invariantId
	 * @returns { Promise<ReportModel>}
	 */
	async getByInvariantIdAsync(invariantId) {
		for (let provider of providers) {
			let report = await provider.getByInvariantIdAsync(invariantId);
			if (report)
				return report;
		}
		return null;
	}
}

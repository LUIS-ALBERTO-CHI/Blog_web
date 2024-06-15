/**
 * @type {Array<{type: String, component: () => Promise<Vue>, getDescription: ()=> { label: String, icon: String }  }>}
 *  
 * */
let reportDisplayType = [];

export default {
	addReportDisplay(type) {
		reportDisplayType.push(type);
	},
	getAll() {
		return reportDisplayType;
	},
	get(type) {
		return reportDisplayType.find(v => v.type === type);
	},
	getFirstDefaultProvider() {
		return reportDisplayType.find(v => v.isDefault);
	}
}
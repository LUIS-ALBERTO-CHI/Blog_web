let customColumnTypes = [];

export default {
	register(type) {
		customColumnTypes.push(type);
	},
	getAll() {
		return customColumnTypes;
	},
	get(type) {
		return customColumnTypes.find(v => v.type === type);
	}
}
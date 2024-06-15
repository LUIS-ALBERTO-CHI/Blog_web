/** @type {Array<Formatter>} */
const formatters = [];

export default {

	/** @param {Formatter} formatter */
	add(formatter) {
		formatters.push(formatter);
	},

	/**@param {String} key
	 * @returns {Formatter} */
	get(key) {
		return formatters.find(f => f.key === key);
	},

	/**@param {String} key
	 * @param {any} value
	 * @returns {Promise<String> } */
	async formatAsync(key, value) {
		const formatter = this.get(key);
		if (formatter)
			return await formatter.formatAsync(value);
		return value?.toString();
	}
}

export class Formatter {

	/** @type {String} */
	key;

	/** @type {(value: any) => Promise<string> }*/
	formatAsync;
}
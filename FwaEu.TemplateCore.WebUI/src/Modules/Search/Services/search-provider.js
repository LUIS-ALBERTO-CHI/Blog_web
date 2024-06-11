export class SearchProvider {

	/** @type {String} */
	key;

	/** @type {String} */
	displayName;

	/** @type {String} */
	icon;

	/** @type {() => Promise<Boolean>} */
	isAvailableAsync;

	/**@type {(results: Array, context: import("./search-context").SearchContext) => Promise<Array<import("./search-result-item").SearchResultItem>>}*/
	processResultsAsync;
}
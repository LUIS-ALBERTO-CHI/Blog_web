export class SearchContext {

	/**@param {import("vue-router").default} router
	 * @param {import("vue-i18n").default} i18n */
	constructor(router, i18n) {
		this.router = router;
		this.i18n = i18n;
	}

	/** @type {import("vue-router").default} */
	router;

	/** @type {import("vue-i18n").default} */
	i18n;

	/** @type {Array<{search: {key: String, skip: Number, take: Number}, results: Array<import("./search-raw-result-group").SearchRawResultGroup>}>} */
	previousSearch;
}
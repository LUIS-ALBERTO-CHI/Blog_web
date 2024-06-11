/**@typedef {import("./search-provider").SerchProvider} SerchProvider
 * @typedef {import("./search-result-item").SearchResultItem} SearchResultItem
 * @typedef {import("./search-raw-result-group").SearchRawResultGroup} SearchRawResultGroup
 * @typedef {import("./search-context").SearchContext} SearchContext */

import HttpService from '@/Fwamework/Core/Services/http-service';
import { Configuration } from '@/Fwamework/Core/Services/configuration-service';

const searchConfiguration = Configuration.search ?? { defaultPageSize: 3, providers: [] };
export const SearchProviderKeySeparator = ':';

/**@type {Array<SerchProvider>} */
const searchProviders = [];

export default {

	/**@param {SerchProvider} provider */
	register(provider) {
		searchProviders.push(provider);
	},

	async getSearchProvidersAsync() {
		const availableProviders = (await Promise.all(searchProviders.map(async p => {
			if (await p.isAvailableAsync()) {
				return p;
			}
		}))).filter(p => p);

		return availableProviders;
	},

	/**@param {String} search
	 * @param {SearchContext} context
	 * @returns {Promise<{results: Array<SearchResultItem>, search: String, canLoadMore: Boolean}>}*/
	async searchAsync(search, context) {
		const result = {
			results: [],
			search: search,
			canLoadMore: false
		};
		search = search?.trim();
		if (!search)
			return result;

		const searchParameters = buildSearchParameters(search, context);
		if (searchParameters.parameters.length === 0 || searchParameters.search.length === 0)
			return result;

		const response = await HttpService.post(`Search/${searchParameters.search}`, searchParameters.parameters);

		/**@type {Array<SearchRawResultGroup>} */
		const groupedSearchResults = response.data;

		result.results = await processResultsAsync(groupedSearchResults, context);

		context.previousSearch = searchParameters.parameters.map(sp => ({ search: sp, results: groupedSearchResults.find(sr => sr.key === sp.key).results }));

		result.search = searchParameters.search;
		result.canLoadMore = context.previousSearch.some(lr => lr.results.length >= lr.search.take);

		return result;
	}
}

/**@param {String} search
 * @param {SearchContext} context
 * @returns {{search: String, parameters: Array<{key: String, skip: Number, take: Number}>}}
 */
function buildSearchParameters(search, context) {

	const availableSearchProviderParameters = context.previousSearch?.filter(lr => lr.results.length >= lr.search.take)
		.map(lr => ({
			key: lr.search.key,
			skip: lr.search.skip + lr.search.take,
			take: lr.search.take
		}))
		?? searchProviders.map(p => ({ key: p.key, skip: 0, take: getPageSize(p.key) }));

	const searchParts = search.split(SearchProviderKeySeparator);
	let currentSearchParameters = { search: search, parameters: availableSearchProviderParameters };

	if (searchParts.length > 1) {

		const requestedSearchProviderKey = searchParts[0];
		const foundSearchProviderParameter = availableSearchProviderParameters.find(sp => sp.key.toLowerCase() == requestedSearchProviderKey.trim().toLowerCase());
		if (foundSearchProviderParameter) {

			currentSearchParameters.search = search.substring(requestedSearchProviderKey.length + 1, search.length).trim();
			currentSearchParameters.parameters = [foundSearchProviderParameter];
		}
	}

	return currentSearchParameters;
}

/** @param {String} searchProviderKey  @returns {Number} */
function getPageSize(searchProviderKey) {
	return 3 || (searchConfiguration.providers.find(p => p.key === searchProviderKey)?.pageSize ?? searchConfiguration.defaultPageSize);
}

/**@param {Array<SearchRawResultGroup>} resultGroups
* @param {SearchContext} searchContext
* @returns {Promise<Array<SearchResultItem>>} */
async function processResultsAsync(resultGroups, searchContext) {
	const processResultPromises = resultGroups.map(r => {
		const provider = searchProviders.find(p => p.key === r.key);

		if (!provider)
			throw new Error(`Search provider '${r.key}' not found`);

		return provider.processResultsAsync(r.results, searchContext);
	});
	const processedResults = (await Promise.all(processResultPromises)).flat();

	return processedResults;
}
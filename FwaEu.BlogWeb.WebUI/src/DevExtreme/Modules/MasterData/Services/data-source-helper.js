
export default {
	/**
	* @param {import('devextreme/data/load_options').LoadOptions} loadOptions
	*/
	getMasterDataParametersFromLoadOptions(loadOptions) {

		let parameters = {};
		parameters.search = loadOptions.searchValue;

		//Load pagination options
		if (loadOptions.take) {
			parameters.pagination = {
				skip: loadOptions.skip || 0,
				take: loadOptions.take
			};
		}

		//Load orderBy options
		if (loadOptions.sort) {
			parameters.orderBy = loadOptions.sort.map(s => {
				return { propertyName: s.selector, ascending: !s.desc };
			});
		}

		return parameters;
	}
}
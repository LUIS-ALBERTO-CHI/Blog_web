export const generalInformationKey = "generalInformation";
export const dataAndFiltersKey = "dataAndFilters";
export const loadDataSourceKey = "loadDataSource";
export const propertiesKey = "properties";

export default {
	getAccordionItems() {
		return [{
			index: 0,
			key: generalInformationKey,
			disabled: false,
		},
		{
			index: 1,
			key: dataAndFiltersKey,
			disabled: true,
		},
		{
			index: 2,
			key: loadDataSourceKey,
			disabled: true,
			visible:true, //NOTE: create property for instance
		},
		{
			index: 3,
			key: propertiesKey,
			disabled: true,
		}]
	},
	getEmptyReportDataObject() {
		return {
			"invariantId": null,
			"name": {},
			"description": {},
			"categoryInvariantId": null,
			"dataSource": {
				"type": null,
				"argument": null
			},
			"navigation":
			{
				"menu": {
					"visible": false,
					"index": 0
				},
				"summary": {
					"visible": false,
					"index": 0
				}
			},
			"filters": [],
			"properties": [],
			"defaultViews": {
				"grid": [],
				"pivot": []
			}
		}
	},

	async validatePanelAsync(item, componentRef) {
		if (!componentRef.validateAsync)
			throw new Error("validate method must be implemented in component " + item.key);
		return await componentRef.validateAsync();  
	},

}


import { ref } from 'vue';
import FarmTownsMasterDataService, { TownsDataSourceOptions } from '@/BlogWeb/FarmManager/Services/farm-towns-master-data-service';

const items = Array.from({ length: 1000 }, (_, i) => ({ label: `Item #${i}`, value: i }));
let itemsLazy = ref(Array.from({ length: 6650 }));
const loading = ref(false);


const loadLazyTimeout = ref();

const getInitial = async (search) => {
	const dsopt = await FarmTownsMasterDataService.getByIdsAsync([search]);
	itemsLazy.value[0] = dsopt[0];
}



const onFilter = function (event) {
	itemsLazy = dsOptions.filter((e) => e.name == event.value);
}

const onLazyLoad = async function (event) {

	loading.value = true;

	if (loadLazyTimeout.value) {
		clearTimeout(loadLazyTimeout.value);
	}

	const searchParams = {
		pagination: {
			skip: itemsLazy.value[event.last - 1] ? event.last - 1 : event.first,
			take: 38,
		}
	}

	if (searchParams.pagination.skip > itemsLazy.value.length) {
		return;
	}

	let _items = [...itemsLazy.value];
	let itemsResults = [].concat(await FarmTownsMasterDataService.getAllRemoteAsync(searchParams));

	let counter = 0;
	if (itemsResults.length > 0) {

		for (let i = event.first + 1; i < event.last + 1; i++) {
			_items[i] = itemsResults[counter];
			counter++;
		}
		itemsLazy.value = _items;

	}

	loading.value = false;
}


const onFilterLazy = async function (event) {
	loading.value = true;
	const searchParams = {
		search: event.value,
		pagination: {
			skip: 0,
			take: 38,
		}
	}

	let _items = [...itemsLazy.value];
	let itemsResults = [].concat(await FarmTownsMasterDataService.getAllRemoteAsync(searchParams));


	let counter = 0;
	if (itemsResults.length > 0) {

		for (let i = 0 + 1; i < itemsResults.length + 1; i++) {
			_items[i] = itemsResults[counter];
			counter++;
		}
		itemsLazy.value = _items;

	}
	loading.value = false;
}


const virtualScrollerOptions = {
	itemSize: 38,
	delay: 300,
	lazy: true,
	//showLoader: true,
	onLazyLoad,
	loading,
	loadLazyTimeout,
	byKey: async (value) => {
		await getInitial(value);
	}
}
export const select4 = {
	"dataField": "select4",
	"label": {
		"text": "lazySelect4"
	},
	"visibleIndex": 1,
	"editorType": "dxSelectBox",
	editorOptions: TownsDataSourceOptions

};
export const firstName = {
	"dataField": "firstName",
	"visibleIndex": 11,
	"label": {
		"text": "firstName"

	},
	"validationRules": [{
		"type": "required"
	}
	]
};
export const lastName = {
	"dataField": "lastName",
	"visibleIndex": 11,
	"label": {
		"text": "lastName"
	},
	"validationRules": [
		{
			"type": "required"
		}
	]
};
export const select = {
	"dataField": "select",
	"label": {
		"text": "select"
	},
	"visibleIndex": 14,
	"editorType": "dxSelectBox",
	"editorOptions": {
		options: [
			{ label: "value1", value: "value1" },
			{ label: "value2", value: "value2" },
			{ label: "value3", value: "value3" },
			{ label: "value4", value: "value4" },
			{ label: "value5", value: "value5" },
			{ label: "value6", value: "value6" },
			{ label: "value7", value: "value7" },
			{ label: "value8", value: "value8" },
			{ label: "value9", value: "value9" },
			{ label: "value10", value: "value10" },
			{ label: "value11", value: "value11" },
			{ label: "value12", value: "value12" },
			{ label: "value13", value: "value13" },
			{ label: "value14", value: "value15" },
			{ label: "value15", value: "value15" },
			{ label: "value16", value: "value16" },
			{ label: "value17", value: "value17" }
		],
		optionLabel: "label",
		optionValue: "value",
	}

};
export const select2 = {
	"dataField": "select2",
	"label": {
		"text": "virtualScrollSelect2"
	},
	"visibleIndex": 14,
	"editorType": "dxSelectBox",
	"editorOptions": {
		options: items,
		virtualScrollerOptions: {
			itemSize: 38
		},
		optionLabel: "label",
		optionValue: "value",
	}

};
export const select3 = {
	"dataField": "select3",
	"label": {
		"text": "lazySelect3"
	},
	"visibleIndex": 14,
	"editorType": "dxSelectBox",
	"editorOptions": {
		options: itemsLazy,
		filter: true,
		onFilter: onFilterLazy,
		onCreated: function (componentValues) {
			this.virtualScrollerOptions.byKey(componentValues.value);
		},
		editable: true,
		virtualScrollerOptions,
		optionLabel: "name",
		optionValue: "id",
	}

};
export const number = {
	"dataField": "number",
	"editorType": "dxNumberBox",
	"visibleIndex": 11,
	"label": {
		"text": "Number"
	},
	"validationRules": [
		{
			"type": "required"
		}
	]
};
export const email = {
	"dataField": "email",
	"visibleIndex": 11,
	"label": {
		"text": "email"
	},
	"validationRules": [
		{
			"type": "required"
		},
		{
			"type": "email"
		}
	]
};
export const createdOn = {
	"dataField": "createdOn",
	"visibleIndex": 11,
	"editorType": "dxDateBox",
	"label": {
		"text": "Created on"
	}
};
export const dataFormItemsArray = [
	select4,
	firstName,
	lastName,
	select,
	select2,
	select3,
	number,
	email,
	createdOn];

export const dataFormItems = {
	select4,
	firstName,
	lastName,
	select,
	select2,
	select3,
	number,
	email,
	createdOn
}

export const dataItems = {
	firstName: "Rudy",
	lastName: "Carrillo",
	select: "Selected",
	number: 8,
	createdOn: new Date(),
	select: "value5",
	select2: 500,
	select3: 2,
	select4: 3
};


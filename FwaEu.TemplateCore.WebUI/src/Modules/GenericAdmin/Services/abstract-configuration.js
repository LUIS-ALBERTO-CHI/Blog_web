import HttpService from '@/Fwamework/Core/Services/http-service';
import ColumnsCustomizerService from '@/Modules/GenericAdmin/Services/columns-customizer-service';
import StringHelperService from '@/Modules/GenericAdmin/Services/string-helper-service';
import DataGridHelperService from '@UILibrary/Modules/GenericAdmin/Services/data-grid-helper-service';
import DotNetTypesToDevExtremeConverterService from '@UILibrary/Extensions/Services/dot-net-types-to-devextreme-converter-service.js';
import LocalizationService from '@/Fwamework/Culture/Services/localization-service';
import merge from 'lodash/merge';

class AbstractConfiguration
{
	constructor(isPaginated)
	{
		if (this.constructor === AbstractConfiguration)
			throw new TypeError('Abstract class "AbstractConfiguration" cannot be instantiated directly');
		this.configuration = null;
		this.fullDataLoad = true;
		this.dataSourceOptions = {
			isPaginated: isPaginated ?? false
		};
		this.baseModel = null;
		this.newModelWithDefaultValues = null;
		this.baseModelWithOnlyKeys = null;
		this.columnsCustomizer = ColumnsCustomizerService.getCustomizer();
		this.customize();
	}

	async loadMessagesAsync(i18n)
	{
	}

	initLocalizableStringsColumnsCustomization()
	{
		let addedValue = 0;
		this.columnsCustomizer.addCustomization('text',
			{ index: 100 });
		this.columnsCustomizer.addCustomization('name',
			{ index: 100 });
		this.columnsCustomizer.addCustomization('description',
			{ index: 200 });

		const defaultLanguageCode = LocalizationService.getDefaultLanguageCode();

		LocalizationService.getSupportedLanguagesCode().forEach(languageCode =>
		{
			let valueToAdd = languageCode === defaultLanguageCode ? 0 : ++addedValue;

			this.columnsCustomizer.addCustomization(`text.${languageCode}`,
				{ index: 100 + valueToAdd });
			this.columnsCustomizer.addCustomization(`name.${languageCode}`,
				{ index: 100 + valueToAdd });
			this.columnsCustomizer.addCustomization(`description.${languageCode}`,
				{ index: 200 + valueToAdd });
		});
	}

	customize()
	{
		this.columnsCustomizer.addCustomization('id', { index: 20, width: 60 });
		this.columnsCustomizer.addCustomization('code', { index: 21 });
		this.columnsCustomizer.addCustomization('invariantId', { index: 21 });
		this.columnsCustomizer.addCustomization('value', { index: 21 });

		this.initLocalizableStringsColumnsCustomization();

		this.columnsCustomizer.addCustomization('isActive', { index: 2000 });

		this.columnsCustomizer.addCustomization('updatedOn', { index: 2100 });
		this.columnsCustomizer.addCustomization('updatedById', { index: 2101, width: 200 });
		this.columnsCustomizer.addCustomization('createdOn', { index: 2200 });
		this.columnsCustomizer.addCustomization('createdById', { index: 2201, width: 200 });
	}

	getApiViewContext()//NOTE: CRUD operations customizer, check wiki for more details
	{
		return null;
	}

	onComponentCreated(component)
	{
		//TODO: Implement me https://dev.azure.com/fwaeu/TemplateWebApplication/_workitems/edit/2111
	}

	initBaseModels()
	{
		this.baseModel = {};
		this.baseModelWithOnlyKeys = {};
		this.newModelWithDefaultValues = {};

		this.configuration.properties.forEach(p =>
		{
			const propertyNameWithFirstCharacterLowered = StringHelperService.lowerFirstCharacter(p.name);
			this.baseModel[propertyNameWithFirstCharacterLowered] = null;
			if (p.isKey)
			{
				this.baseModelWithOnlyKeys[propertyNameWithFirstCharacterLowered] = null;
			}
			let dotNetTypeToDevExtremeConverter = DotNetTypesToDevExtremeConverterService.getConverter(p.type);

			this.newModelWithDefaultValues[propertyNameWithFirstCharacterLowered]
				= dotNetTypeToDevExtremeConverter ? dotNetTypeToDevExtremeConverter.getDefaultValue(p) : null;
		});
	}

	onConfigurationLoaded(component)
	{
		this.configuration = component.configuration;
		this.initBaseModels();
	}

	onInitDataSource(configuration)
	{
		return configuration;
	}

	createDataSourceOptions(component)
	{
		const viewContext = this.getApiViewContext();
		const isPaginated = this.dataSourceOptions.isPaginated;
		const thisConfiguration = this;

		function saveModel(event) {
			component.genericAdminConfiguration.onBeforeCreateOrUpdateRequest(component, event);
			return HttpService.post('GenericAdmin/Save/' + component.configurationKey,
				{
					models: [event.data],
					viewContext: { value: viewContext }
				});
		}

		return {
			key: component.configuration.properties.filter(p => p.isKey).reduce((agg, p) =>
			{
				agg.push(StringHelperService.lowerFirstCharacter(p.name));
				return agg;
			}, []),
			load: function (loadOptions)
			{
				if (!thisConfiguration.fullDataLoad) {
					thisConfiguration.fullDataLoad = true;
					return new Promise((resolve) => {
						resolve(component.dataSource);
					});
				}

				const options = {
					pagination: null,
					sort: null,
					filter: null
				};

				if (loadOptions.requireTotalCount != null ||
					loadOptions.skip != null ||
					loadOptions.take != null) {
					options.pagination = {
						requireTotalCount: loadOptions.requireTotalCount,
						skip: loadOptions.skip,
						take: loadOptions.take,
					};
				}

				if (loadOptions.sort?.length > 0) {
					options.sort = {
						parameters: loadOptions.sort?.map(s => ({
							columnName: s.selector,
							ascending: s.desc != true
						}))
					};
				}

				if (loadOptions.filter?.length > 0) {
					options.filter = {
						filters: loadOptions.filter
					};
				}

				return HttpService.post('GenericAdmin/GetModels/' + component.configurationKey, options).then(result => {
					component.dataSource = result.data;
					return component.dataSource;
				});
			},
			insert: function (newData)
			{
				return saveModel({ data: newData }).then(result => {
					thisConfiguration.fullDataLoad = isPaginated;
					if (!isPaginated) {
						component.dataSource.data.push(...result.data.results.map(r => r.savedData));
					}
				});
			},
			update: function (key, data)
			{
				return saveModel({ data: data }).then(result => {
					thisConfiguration.fullDataLoad = isPaginated;
					if (!isPaginated) {
						Object.keys(data).forEach((k) => {
							data[k] = result.data.results[0].savedData[k];
						});
					}
				});
			},
			remove: function (keys)
			{
				return HttpService.post('GenericAdmin/Delete/' + component.configurationKey, {
					keys: [keys],
					viewContext: { value: viewContext }
				}).then(result => {
					thisConfiguration.fullDataLoad = isPaginated;

					if (!isPaginated) {
						component.dataSource.data = component.dataSource.data.filter(i => !result.data.results.some(r => {
							return Object.entries(r.keys).every(([k, v]) => i[k] === v);
						}));
					}

					return result;
				});
			}
		};
	}

	createDataSource(component)
	{
		return DataGridHelperService.createCustomStore(this.createDataSourceOptions(component));
	}

	onBeforeCreateOrUpdateRequest(component, event)
	{
		event.data = merge(this.baseModel, event.data);
	}

	async onColumnsCreatingAsync(component, columns)
	{
		await this.columnsCustomizer.customizeAsync(columns, this.configuration.properties);
	}

	onInitNewRow(component, event)
	{
		event.data = merge(this.newModelWithDefaultValues, event.data);
	}

	onRowInserting(component, event)
	{
	}

	onRowInserted(component, event)
	{
	}

	onInitialized(component, event)
	{

	}

	onRowUpdating(component, event)
	{
		event.newData = merge(event.oldData, event.newData);
	}

	onRowUpdated(component, event)
	{
	}

	onRowRemoving(component, event)
	{
	}

	onRowRemoved(component, event)
	{
	}

	getExportImplementation(component)
	{
		//NOTE: For custom implementation, check https://supportcenter.devexpress.com/Ticket/Details/T315513/dxdatagrid-how-to-get-filtered-data
		return null;
	}

	getExportFileNameKey()
	{
		return this.getPageTitleKey();
	}

	getPageTitleKey()
	{
		throw new Error('You have to implement the method getPageTitle!');
	}

	getPageContainerCustomClass()
	{
		return '';//NOTE: PageContainerComponent does not handle null or undefined value, would add "null" as class
	}

	getDescriptionKey()
	{
		return null;
	}

	getGroupTextKey()
	{
		return 'genericAdminReferentials';
	}

	getGroupIndex() {
		return 100;
	}

	getVisibleIndex() {
		return 0;
	}
}

export default AbstractConfiguration;
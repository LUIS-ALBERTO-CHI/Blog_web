import AbstractCustomColumnCustomizer from '@/Modules/GenericAdmin/Services/abstract-custom-column-customizer';
import DataSourceOptionsFactory from "@UILibrary/Modules/MasterData/Services/data-source-options-factory";
import StringHelperService from '@/Modules/GenericAdmin/Services/string-helper-service';
import MasterDataManagerService from '@/Fwamework/MasterData/Services/master-data-manager-service';

class MasterDataColumnCustomizer extends AbstractCustomColumnCustomizer {
	async customizeAsync(columns, properties) {
		await super.customizeAsync(columns, properties);

		for (const column of columns) {
			const propertyExtendedProperties = properties.find(p => StringHelperService.lowerFirstCharacter(p.name) === column.name)?.extendedProperties;
			const masterDataName = propertyExtendedProperties?.masterData?.name;
			if (masterDataName) {
				const masterDataService = MasterDataManagerService.getMasterDataService(masterDataName);

				if (!masterDataService.configuration.fullLoad) {
					column.editorOptions.placeholder = "";
					column.showEditorAlways = true;//NOTE: DxDataGrid requires it for server processed data sources https://supportcenter.devexpress.com/ticket/details/t672141/datagrid-how-to-reduce-the-number-of-loaded-items-in-a-lookup-column#approvedAnswers
				}

				column.lookup = {
					dataSource: DataSourceOptionsFactory.create(masterDataService),
					valueExpr: masterDataService.configuration.keyHelper.getItemKey,
					displayExpr(item) {
						return item?.toString();
					}
				}
			}
		}
	}
}

export default MasterDataColumnCustomizer;
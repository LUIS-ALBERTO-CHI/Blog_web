import EnumMasterDataService from '@/Fwamework/EnumMasterData/Services/enum-master-data-service';
import DataSourceOptionsFactory from "@UILibrary/Modules/MasterData/Services/data-source-options-factory";

const FarmCategorySizeMasterDataService = new EnumMasterDataService('FwaEu.BlogWeb.FarmManager.Entities.FarmCategorySize');

export default FarmCategorySizeMasterDataService;
export const FarmCategorySizeDataSourceOptions = DataSourceOptionsFactory.create(FarmCategorySizeMasterDataService);

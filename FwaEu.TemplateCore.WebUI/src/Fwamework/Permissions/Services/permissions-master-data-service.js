import MasterDataService from "@/Fwamework/MasterData/Services/master-data-service";
import DataSourceOptionsFactory from "@UILibrary/Modules/MasterData/Services/data-source-options-factory";
import { I18n } from '@/Fwamework/Culture/Services/localization-service';

class PermissionsMasterDataService extends MasterDataService {
	constructor() {
		super('Permissions', ['invariantId']);
	}

	createItem(permission) {
		return {
			...permission,
			name: I18n.t(permission.invariantId),
			toString() {
				return this.name;
			}
		};
	}
}
const masterDataService = new PermissionsMasterDataService();

export default masterDataService;
export const PermissionsDataSourceOptions = DataSourceOptionsFactory.create(masterDataService);

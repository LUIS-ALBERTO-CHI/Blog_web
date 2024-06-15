import MasterDataService from "@/Fwamework/MasterData/Services/master-data-service";
import UserFormatterService from "@/Fwamework/Users/Services/user-formatter-service";
import DataSourceOptionsFactory from "@UILibrary/Modules/MasterData/Services/data-source-options-factory";

class UsersMasterDataService extends MasterDataService {
	constructor() {
		super('Users', ['id']);
	}

	createItem(user) {
		return {
			...user,
			fullName: UserFormatterService.getUserFullName(user),
			toString() {
				return this.fullName;
			}
		}
	}
}

const masterDataService = new UsersMasterDataService();

export default masterDataService;
export const UsersDataSourceOptions = DataSourceOptionsFactory.create(masterDataService);

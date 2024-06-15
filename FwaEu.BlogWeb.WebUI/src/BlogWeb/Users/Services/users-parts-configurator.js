import CulturePart from '@/Modules/UserCulture/Components/culture-user-part';
import HistoryPart from '@/Modules/UserHistory/Components/history-user-part';
import PerimeterPart from '@/Modules/UserPerimeter/Components/perimeter-user-part';
import RolePart from '@/Modules/Roles/Components/role-user-part';
//import PermissionPart from '@/Modules/PermissionsByUser/Components/permission-user-part';
import AdminStatePart from '@/Modules/UserAdminState/Components/admin-state-user-part';
import UserPartsRegistry from '@/Fwamework/Users/Services/users-parts-registry';
import UserApplicationUserPart from './user-application-user-part';

export default {

	configure() {
		UserPartsRegistry.addUserPart(CulturePart);
		UserPartsRegistry.addUserPart(PerimeterPart);
		UserPartsRegistry.addUserPart(RolePart); //TODO: Choose between ByRole and others implementations
		//UserPartsRegistry.addUserPart(PermissionPart);//TODO: Choose between ByUser and others implementations
		UserPartsRegistry.addUserPart(HistoryPart);
		UserPartsRegistry.addUserPart(AdminStatePart);
		UserPartsRegistry.addUserPart(UserApplicationUserPart);
	}
}
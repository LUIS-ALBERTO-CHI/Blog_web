import RolesMasterDataService from "@/Modules/Roles/Services/roles-master-data-service";
import { I18n } from "@/Fwamework/Culture/Services/localization-service";
import { hasPermissionAsync } from "@/Fwamework/Permissions/Services/current-user-permissions-service";
import { CanAdministrateUsers } from "@/Fwamework/Users/users-permissions";
import { defineAsyncComponent } from "vue";
import NotificationService from '@/Fwamework/Notifications/Services/notification-service';

export default {
	partName: "roles",
	component: defineAsyncComponent(() => import("@UILibrary/Modules/Roles/Components/RoleUserPartComponent.vue")),

	initializeAsync: function (user, context) {

		let userPart = user.parts[this.partName];
		if (context.isNew) {
			userPart = createNewUserPart();
		}
		return [{
			showForAdmin: false,
			readOnly: context.currentUser.id === user.id,
			data: userPart,
			title: I18n.t('roleTitle'),
			fetchDataAsync: async () => await RolesMasterDataService.getAllAsync()
		}];
	},

	fillAsync: async function (user, component, context) {
		const hasPermission = user.id !== context.currentUser.id
			&&
			await hasPermissionAsync(CanAdministrateUsers)
		if (hasPermission) {
			user.parts[this.partName] = { selectedIds: component.modelValue.data.selectedIds };
		}
	}
}


function createNewUserPart() {
	return { selectedIds: [] };
}
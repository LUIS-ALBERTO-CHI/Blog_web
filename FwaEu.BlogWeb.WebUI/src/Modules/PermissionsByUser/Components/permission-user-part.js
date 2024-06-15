import PermissionsMasterDataService from "@/Fwamework/Permissions/Services/permissions-master-data-service"
import { I18n } from "@/Fwamework/Culture/Services/localization-service";
import { hasPermissionAsync } from "@/Fwamework/Permissions/Services/current-user-permissions-service";
import { CanAdministrateUsers } from "@/Fwamework/Users/users-permissions";
import { defineAsyncComponent } from "vue";
import NotificationService from '@/Fwamework/Notifications/Services/notification-service';

export default {
	partName: "permissions",
	component: defineAsyncComponent(() => import("@UILibrary/Modules/PermissionsByUser/Components/PermissionUserPartComponent.vue")),
	initializeAsync: function (user, context) {

		let userPart = user.parts[this.partName];
		if (context.isNew) {
			userPart = createNewUserPart();
		}
		return [{
			showForAdmin: false,
			readOnly: context.currentUser.id === user.id,
			data: userPart,
			title: I18n.t('permissionTitle'),
			fetchDataAsync: async () => await PermissionsMasterDataService.getAllAsync()
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
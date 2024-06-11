import UserPerimeterProviderService from '@/Modules/UserPerimeter/Services/user-perimeter-provider-service';
import { hasPermissionAsync } from '@/Fwamework/Permissions/Services/current-user-permissions-service';
import { CanAdministrateUsers } from '@/Fwamework/Users/users-permissions';
import { defineAsyncComponent } from 'vue';
import { I18n } from "@/Fwamework/Culture/Services/localization-service";
import NotificationService from '@/Fwamework/Notifications/Services/notification-service';

export default {
	partName: "perimeters",
	component: defineAsyncComponent(() => import("@UILibrary/Modules/UserPerimeter/Components/UserPerimeterPartComponent.vue")),

	async initializeAsync(user, context) {
		const $this = this;
		function getDataByPerimeterKey(key) {
			if (context.isNew) {
				//NOTE: Displaying the user form to create a new user
				return createNewUserPart(key);
			}
			else { //NOTE: Displaying the user form for modification of an existing user
				return user.parts[$this.partName].entries.find(e => e.key == key) || createNewUserPart(key);
			}
		}
		function getCanGrantFullAccess(key) {
			return context.currentUser.id !== user.id &&
				context.currentUser.parts.perimeters.entries.find(e => e.key == key)?.hasFullAccess;
		}
		
		return Promise.all(UserPerimeterProviderService.getAllPerimeterTypeProviders()
			.map(async function (perimeterTypeProvider) {
				return (typeof perimeterTypeProvider.isPerimeterAccessibleAsync !== 'function' || await perimeterTypeProvider.isPerimeterAccessibleAsync())
					? {
						showForAdmin: false,
						readOnly: context.currentUser.id === user.id,
						canGrantFullAccess: getCanGrantFullAccess(perimeterTypeProvider.key),
						data: getDataByPerimeterKey(perimeterTypeProvider.key),
						title: I18n.t(perimeterTypeProvider.titleKey),
						fetchDataAsync: async () => await perimeterTypeProvider.fetchDataAsync()
					} : null;
			})
		).then(models => models.filter(m => m));//NOT: Filter not accessible perimeter types
	},

	fillAsync: async function (user, component, context) {
		//NOTE: This method will be called once per model entry
		const hasPermission = user.id !== context.currentUser.id
			&&
			await hasPermissionAsync(CanAdministrateUsers)
		if (hasPermission) {
			if (!user.parts[this.partName]) {
				user.parts[this.partName] = { entries: [] };
			}
			let entries = user.parts[this.partName].entries;
			const hasFullAccess = component.modelValue.data.hasFullAccess;
			let entry = {
				key: component.modelValue.data.key,
				hasFullAccess: hasFullAccess,
				accessibleIds: hasFullAccess ? null : component.modelValue.data.accessibleIds
			};
			entries.push(entry);
		}
	}
}

function createNewUserPart(key) {
	return { key: key, hasFullAccess: false, accessibleIds: [] };
}

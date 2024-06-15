import { I18n } from "@/Fwamework/Culture/Services/localization-service";
import { CanAdministrateUsers } from '@/Fwamework/Users/users-permissions';
import { hasPermissionAsync } from '@/Fwamework/Permissions/Services/current-user-permissions-service';
import NotificationService from '@/Fwamework/Notifications/Services/notification-service';
import AuthenticationService from "@/Fwamework/Authentication/Services/authentication-service";

export default {
	partName: "credentials",

	initializeAsync: function (user, context) {

		const userPart = createNewUserPart();//Credentials part is never included for security reasons, we need to initialize it always
		return [{
			data: userPart,
			isPasswordRequired: context.isNew,
			location: 'general-information',
			createFormItemsAsync: createFormItemsAsync,
		}];
	},

	fillAsync: async function (user, component, context) {
		if (context.formModel.newPassword) {
			const hasPermission = user.id === context.currentUser.id
				||
				await hasPermissionAsync(CanAdministrateUsers);
			if (hasPermission) {
				user.parts[this.partName] = {
					newPassword: context.formModel.newPassword
				};
			}
		}
	},

	async onUserSavedAsync(user, context) {
		if (user.parts[this.partName] && user.id === context.currentUser.id) {
			NotificationService.showInformation(I18n.t("passwordChangedConfirmationMessage"));
			await AuthenticationService.logoutAsync();
		}
	}
}

async function createFormItemsAsync(user, context) {
	return [{
		dataField: "newPassword",
		label: {
			text: I18n.t(context.isNew
				? 'newUserPassword'
				: 'existingUserNewPassword')
		},
		visibleIndex: 50,
		editorOptions: {
			mode: 'password'
		},
		validationRules: context.isNew
			? [{ type: "required" }]
			: null
	}];
}

function createNewUserPart() {
	return {
		newPassword: null
	};
}


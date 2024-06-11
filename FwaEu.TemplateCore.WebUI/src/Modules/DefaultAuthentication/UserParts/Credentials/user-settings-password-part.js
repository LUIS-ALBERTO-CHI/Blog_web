import { I18n } from "@/Fwamework/Culture/Services/localization-service";
import NotificationService from "@/Fwamework/Notifications/Services/notification-service";
import AuthenticationService from "@/Fwamework/Authentication/Services/authentication-service";
import { defineAsyncComponent } from "vue";

export default {
	partName: "credentials",
	component: defineAsyncComponent(() => import("@UILibrary/Modules/DefaultAuthentication/Components/UserSettingsPasswordPartComponent.vue")),

	initializeAsync: function (user, context) {
		const userSettingsPart = createNewUserSettingsPart();
		return [{
			data: userSettingsPart,
			title: I18n.t('passwordSettingTitle')
		}];
	},

	fillAsync: async function (user, component, context) {
		if (component.modelValue.data.newPassword) {
			user.parts[this.partName] = {
				currentPassword: component.modelValue.data.currentPassword,
				newPassword: component.modelValue.data.newPassword
			};
		}
	},

	async onUserSavedAsync(user) {
		if (user.parts[this.partName]) {
			NotificationService.showInformation(I18n.t("passwordChangedConfirmationMessage"));
		}
	}
}

function createNewUserSettingsPart() {
	return {
		currentPassword: null,
		newPassword: null,
		confirmPassword: null
	};
}

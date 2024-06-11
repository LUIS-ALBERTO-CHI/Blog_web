import { I18n } from "@/Fwamework/Culture/Services/localization-service";
import PasswordRecoveryService from './Services/password-recovery-service';
import NotificationService from "@/Fwamework/Notifications/Services/notification-service";
import Dialog from "@UILibrary/Modules/Dialog/Services/dialog-service";
import { defineAsyncComponent } from "vue";
import { AuthenticationHandlerKey } from "../../Services/default-authentication-handler";

export default {
	partName: "passwordRecovery",
	authenticationHandlerKey: AuthenticationHandlerKey,

	async createMenuItemsAsync(user, context) {
		return [{
			icon: 'clearformat',
			text: I18n.t('forgotPassword'),
			action: async function (e) {
				const confirmResult = await Dialog.confirmAsync(I18n.t('confirmationMessage'), I18n.t('confirmationTitle'));
				if (confirmResult) {
					await PasswordRecoveryService.reinitializePasswordAsync(user.email);
					NotificationService.showConfirmation(I18n.t('mailSent'));
				}
			}
		}]
	},
	createLoginComponentAsync: async function () {
		return { component: defineAsyncComponent(() => import('@UILibrary/Modules/PasswordRecovery/Components/PasswordRecoveryRequestComponent.vue')) }
	}
}



 
import LocalizationService, { I18n } from "@/Fwamework/Culture/Services/localization-service";
import { hasPermissionAsync } from "@/Fwamework/Permissions/Services/current-user-permissions-service";
import { CanAdministrateUsers } from "@/Fwamework/Users/users-permissions";
import { createLanguageBox } from "@/Fwamework/Culture/Services/language-box-helper";
import NotificationService from '@/Fwamework/Notifications/Services/notification-service';

const multipleLanguages = LocalizationService.getSupportedLanguages().length > 1;

export default {
	partName: "cultureSettings",

	initializeAsync: function (user, context) {
		let userPart = user.parts[this.partName];

		if (context.isNew || !userPart) {
			userPart = createNewUserPart();
		}
		return [{
			data: userPart,
			location: 'general-information',
			createFormItemsAsync: createFormItemsAsync,
		}];
	},

	fillAsync: async function (user, component, context) {
		const hasPermission = user.id === context.currentUser.id
			||
			await hasPermissionAsync(CanAdministrateUsers);
		if (hasPermission) {
			const languageCodeToSave = multipleLanguages ? context.formModel.languageTwoLetterIsoCode
				: LocalizationService.getDefaultLanguageCode();
			user.parts[this.partName] = {
				languageTwoLetterIsoCode: languageCodeToSave
			};
		}
	},

	async onUserSavedAsync(user, context) {
		if (user.id === context.currentUser.id
			&& user.parts[this.partName]?.languageTwoLetterIsoCode != LocalizationService.getCurrentLanguage()) {
			await LocalizationService.setCurrentLanguageAsync(user.parts[this.partName]?.languageTwoLetterIsoCode);
		}
	}
}

async function createFormItemsAsync(user, context) {

	return multipleLanguages ? [{
		dataField: "languageTwoLetterIsoCode",
		label: {
			text: I18n.t('language')
		},
		visibleIndex: 14,
		editorType: "dxSelectBox",
		validationRules: [{ type: "required" }],
		template: function (args) {
			const componentInstance = createLanguageBox({
				languageValue: user.languageTwoLetterIsoCode,
				//NOTE: onUpdate:languageValue is the equivalent of @language-value or @languageValue (listens update:languageValue event)
				'onUpdate:languageValue': function (newValue) {
					user.languageTwoLetterIsoCode = newValue;
				}
			});
			args.component.on('disposing', function () {
				componentInstance.unmount();
			});
			return componentInstance.$el;
		}
	}] : [];
}

function createNewUserPart() {
	return {
		languageTwoLetterIsoCode: LocalizationService.getCurrentLanguage()
	};
}

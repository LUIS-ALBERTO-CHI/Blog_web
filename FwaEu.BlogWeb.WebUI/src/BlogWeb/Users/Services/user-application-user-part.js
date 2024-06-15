import { I18n } from "@/Fwamework/Culture/Services/localization-service";

export default {
	partName: "application",
	initializeAsync: function (user, context) {

		let userPart = user.parts[this.partName];
		if (context.isNew || !userPart) {
			userPart = createNewUserPart();
		}
		return [{
			handler: this,
			data: userPart,
			location: 'general-information',
			createFormItemsAsync: createFormItemsAsync,
		}];
	},

	fillAsync: async function (user, component, context) {
		user.parts[this.partName] = {
			email: context.formModel.email,
			firstName: context.formModel.firstName,
			lastName: context.formModel.lastName
		};
	}
}

function createFormItemsAsync(user, initializeContext) {
	return [
		{
			dataField: "firstName",
			label: { text: I18n.t('firstName') },
			visibleIndex: 1,
			validationRules: [{ type: "required" }],
			editorOptions: {
				inputAttr: { autocomplete: "nofill" }
			}
		},
		{
			dataField: "lastName",
			label: { text: I18n.t('lastName') },
			visibleIndex: 2,
			validationRules: [{ type: "required" }],
			editorOptions: {
				inputAttr: { autocomplete: "nofill" }
			}
		},
		{
			dataField: "email",
			label: { text: I18n.t('email') },
			visibleIndex: 20,
			validationRules: [{ type: "required" }, { type: "email" }],
			editorOptions: {
				inputAttr: { autocomplete: "nofill" }
			}
		}
	];
}
function createNewUserPart() {
	return {
		email: null,
		firstName: null,
		lastName: null
	};
}

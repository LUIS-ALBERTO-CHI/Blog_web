import { I18n } from "@/Fwamework/Culture/Services/localization-service";

export default {
	partName: "farmer",
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
		user.parts[this.partName] = {
			pseudonym: context.formModel.pseudonym
		};
	}
}

function createFormItemsAsync(user, context) {
	return [
		{
			dataField: "pseudonym",
			visibleIndex: 11,
			label: { text: I18n.t('pseudonym') }
		}
	];
}
function createNewUserPart() {
	return {
		pseudonym: null
	};
}

import { I18n } from "@/Fwamework/Culture/Services/localization-service";
import UserStateService from '@/Fwamework/Users/Services/user-state-service';
import NotificationService from '@/Fwamework/Notifications/Services/notification-service';

export default {
	partName: "adminState",
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
		const canChangeIsAdmin = hasPermission(user, context.currentUser);

		user.parts[this.partName] = {
			state: context.formModel.state
		};
		if (canChangeIsAdmin) {
			user.parts[this.partName].isAdmin = context.formModel.isAdmin;
		}
	}
}
function hasPermission(user, currentUser) {
	return user.id !== currentUser.id && currentUser.parts?.adminState?.isAdmin;
}

function createFormItemsAsync(user, context) {
	const states = UserStateService.getAll();
	const isCurrentUser = user.id === context.currentUser.id;
	const canChangeIsAdmin = hasPermission(user, context.currentUser);

	let formItems = [
		{
			dataField: "state",
			label: { text: I18n.t('state') },
			visibleIndex: 100,
			validationRules: [{ type: "required" }],
			editorType: "dxSelectBox",
			editorOptions: {
				items: states,
				displayExpr: 'text',
				valueExpr: 'value',
				disabled: isCurrentUser,
				inputAttr: { autocomplete: "nofill" }
			}

		},
	];

	if (context.currentUser.parts?.adminState?.isAdmin) {
		formItems.push({
			dataField: "isAdmin",
			label: { text: I18n.t('isAdmin') },
			visibleIndex: 110,
			editorType: "dxSwitch",
			editorOptions: { disabled: !canChangeIsAdmin, text: I18n.t('yes') }
		});
	}

	return formItems;
}
function createNewUserPart() {
	return {
		isAdmin: 0,
		state: null
	};
}

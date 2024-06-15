import notify from 'devextreme/ui/notify';
const position = { my: 'center top', at: 'center top', offset: '0 7.5' }, timer = 3000;

export default {
	exportName: 'NotificationService',
	key: 'DevExtremeNotifier',
	showError(message) {
		return notify({
			message: message,
			position: position
		}, 'error', timer);
	},
	showConfirmation(message) {
		return notify({
			message: message,
			position: position
		}, 'success', timer);
	},
	showWarning(message) {
		return notify({
			message: message,
			position: position
		}, 'warning', timer);
	},
	showInformation(message) {
		return notify({
			message: message,
			position: position
		}, 'info', timer);
	}
};

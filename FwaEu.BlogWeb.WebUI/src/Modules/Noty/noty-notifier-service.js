import Noty from 'noty';
import "noty/lib/noty.css";
import "./Content/noty-styles.css";

const notyDefaultOptions = {
	progressBar: true,
	theme: 'relax',
	timeout: 6666,
	closeWith: ['button'],
	layout: 'topRight',
};

const showNotification = function (message, options, type) {
	if (!options)
		options = {};
	let newOptions = { ...notyDefaultOptions, ...options };
	newOptions.text = message;
	newOptions.type = type;
	return new Noty(newOptions).show();
}

export default {
	exportName: 'NotificationService',
	key: 'Noty',
	showError(message, options) {
		return showNotification(message, options, 'error');
	},
	showConfirmation(message, options) {
		return showNotification(message, options, 'success');
	},
	showWarning(message, options) {
		return showNotification(message, options, 'warning');
	},
	showInformation(message, options) {
		return showNotification(message, options, 'info');
	}
};


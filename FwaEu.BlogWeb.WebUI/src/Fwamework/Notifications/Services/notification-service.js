import { Configuration } from '@/Fwamework/Core/Services/configuration-service';
const context = import.meta.glob('/**/*-notifier-service.js', { import: 'default', eager: true });

const notifierServices = Object.values(context);
const currentNotifier = notifierServices.find(ns => ns.key === Configuration.fwamework.notifications.notifierKey);

export default {

	showError(message, options = null) {
		return currentNotifier.showError(message, options);
    },
	showConfirmation(message, options = null) {
		return currentNotifier.showConfirmation(message, options);
    },
	showWarning(message, options = null) {
		return currentNotifier.showWarning(message, options);
    },

	showInformation(message, options = null) {
		return currentNotifier.showInformation(message, options);
    }
};
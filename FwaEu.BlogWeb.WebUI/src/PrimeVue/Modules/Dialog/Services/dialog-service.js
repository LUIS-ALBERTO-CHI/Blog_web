import { PrimeIcons } from "primevue/api";
import ConfirmationService from 'primevue/confirmationservice';
import DialogService from 'primevue/dialogservice';

export default {
	initialize(vueApp) {
		vueApp.use(DialogService);
		vueApp.use(ConfirmationService);
	},
	configure(vueApp) {
		this.vueApp = vueApp;
	},
	async confirmAsync(message, title = null) {
		const I18n = (await import("@/Fwamework/Culture/Services/localization-service")).I18n;
		return new Promise((resolve, reject) => {

			this.vueApp.$confirm.require({
				header: title,
				message: message,
				icon: PrimeIcons.QUESTION_CIRCLE,
				rejectLabel: I18n.t('no'),
				acceptLabel: I18n.t('yes'),
				accept: () => {
					this.vueApp.$confirm.close();
					resolve(true);
				},
				reject: () => {
					this.vueApp.$confirm.close();
					resolve(false);
				}
			});
		});
	},
	async alertAsync(message, title) {
		const I18n = (await import("@/Fwamework/Culture/Services/localization-service")).I18n;
		this.vueApp.$confirm.require({
			header: title,
			message: message,
			icon: PrimeIcons.INFO_CIRCLE,
			rejectLabel: ' ',
			acceptLabel: I18n.t('ok'),
			accept: () => {
				this.vueApp.$confirm.close();
			},

		});
	},
	/**
	 * @param {import("vue").Component} component
	 * @param {any} options see The PrimeVue Dynamic dialog documentation
	 */
	open(component, options) {
		return this.vueApp.$dialog.open(component, options);
	}
}
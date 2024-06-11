import { waitFor } from 'vue-wait';
import { done, start } from '@/Modules/NavigationIndicator/Services/navigation-indicator-service';
export const ModalLoadingName = "modal_loading";
export const NoModalLoadingName = "no_modal_loading";
import Router from '@/Fwamework/Routing/Services/vue-router-service';

let waitInstance;

let lastWaitId = 1;

const getWaitIdentifier = function (loaderName) {
	return loaderName + (lastWaitId++);
};

export function showLoadingPanel(asyncFunction, loaderName = ModalLoadingName) {

	let waitForFunction = waitInstance ? waitFor.bind(waitInstance.$root) : waitFor;

	return async function (...args) {
		try {
			start();
			let waitIdentifier = getWaitIdentifier(loaderName ?? (this.getLoaderName ? this.getLoaderName() : null) ?? this.loaderName ?? ModalLoadingName);
			return await waitForFunction(waitIdentifier, asyncFunction).apply(this, args);
		} finally {
			done();
		}
	};
}

let lastManualLoaderId = 0;
export default {
	configure(vueWait) {
		waitInstance = vueWait;

		// When each route is finished remove remaining Block UI that didn't finish animation, so PrimeVue doesn't remove them
		Router.afterEach(() => {

			try {
				Array.from(document.getElementsByClassName('p-component-overlay-leave')).forEach(item => item.parentNode.removeChild(item));
			} catch (error) {
				console.error(error);
			}
		});
	},
	showLoadingPanel,
	showModal() {
		const waitName = ModalLoadingName + '_manual_' + (++lastManualLoaderId);
		start();
		waitInstance.start(waitName);

	},
	hideModal() {
		const waitName = ModalLoadingName + '_manual_' + (lastManualLoaderId--);
		waitInstance.end(waitName);
		done();
	}
};
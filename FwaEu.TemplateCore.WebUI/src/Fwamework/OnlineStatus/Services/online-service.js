import AsyncEventEmitter from '@/Fwamework/Core/Services/event-emitter-class';
import { useOnline } from '@vueuse/core';
import { watch } from 'vue';

const onOfflineEventEmitter = new AsyncEventEmitter();
const onOnlineEventEmitter = new AsyncEventEmitter();
const online = useOnline();

export default {

	initialize() {
		watch(online, this.emitOnlineAsync);
	},

	/** @param {() => Promise} listener */
	onOffline(listener) {
		return onOfflineEventEmitter.addListener(listener);
	},

	/** @param {() => Promise} listener */
	onOnline(listener) {
		return onOnlineEventEmitter.addListener(listener);
	},

	async emitOnlineAsync() {

		if (online.value) {
			await onOnlineEventEmitter.emitAsync();

		} else {
			await onOfflineEventEmitter.emitAsync();
		}
	},

	isOnline() {
		return online.value;
	}
}


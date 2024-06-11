import HttpService from '@/Fwamework/Core/Services/http-service';
import { LocalStorage } from '@/Fwamework/Storage/Services/local-storage-store';
import AsyncEventEmitter from '@/Fwamework/Core/Services/event-emitter-class';

const viewContextKey = 'View-Context';

const viewContextChangedEmitter = new AsyncEventEmitter();

export class ViewContextModel {

	/** @param {Number|null} regionId */
	constructor(regionId) {
		this.regionId = regionId;
	}

	/** @type Number|null*/
	regionId = null
}

export default {
	configure() {
		const $this = this;
		HttpService.interceptors.request.use(async config => {

			config.headers.common[viewContextKey] = JSON.stringify($this.get());
			return config;
		});
	},

	onChanged(listener) {
		return viewContextChangedEmitter.addListener(listener);
	},

	/** @param {ViewContextModel} viewContext */
	set(viewContext) {
		LocalStorage.setValue(viewContextKey, viewContext);
		viewContextChangedEmitter.emitAsync(viewContext);
	},

	/** @returns {ViewContextModel} */
	get() {
		const viewContext = LocalStorage.getValue(viewContextKey);
		return viewContext;
	}
}
import AsyncEventEmitter from "@/Fwamework/Core/Services/event-emitter-class";
import { ref, shallowRef } from "vue";

/** @type {import("vue").Ref<Array<import("./header-item").HeaderItem>>} **/
const items = ref([]);

const visibilityChangedEventEmitter = new AsyncEventEmitter();


export default {
	getAllItems() {
		return items;
	},

	register(item) {
		items.value.push({
			...item,
			configuration: {
				...item.configuration,
				component: item.configuration.component ? shallowRef(item.configuration.component) : undefined,
				smallModeContentComponent: item.configuration.smallModeContentComponent ? shallowRef(item.configuration.smallModeContentComponent) : undefined
			}
		});
	},

	setVisibility(key, isVisible) {
		items.value.find(x => x.configuration.key === key).isVisible = isVisible;
		visibilityChangedEventEmitter.emitAsync({ key, isVisible: isVisible });
	},

	onVisibilityChanged(listener) {
		return visibilityChangedEventEmitter.addListener(listener);
	},

	async reloadDataAsync(item) {

		item.fetchedData = await item.configuration.fetchDataAsync();
	}
}
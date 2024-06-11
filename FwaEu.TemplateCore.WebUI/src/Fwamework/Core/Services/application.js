import { createApp } from 'vue';
import ModuleRegistry from "@/Fwamework/Core/Services/module-registry";

export default class Application {

	/** @param {import('vue/types/vue').VueConfiguration } vueConfig */
	constructor(vueConfig) {
		this.vueConfig = vueConfig;
		let originalCreated = this.vueConfig.created;
		this.vueConfig.created = function () {

			const vueInstance = this;
			if (originalCreated)
				originalCreated.bind(vueInstance)();

			ModuleRegistry.getAll().forEach(function (module) {

				/**************** Call onApplicationCreated for all aplication modules ****************/
				//onApplicationCreated can be async but we won't wait for it because vuejs created event cannot be asynchronous
				module.onApplicationCreated(vueInstance);
			});
		};
		this.vueApp = createApp(this.vueConfig);
	}

	/** @type {import('vue').App} */
	vueApp

	/** @type {import('vue/types/vue').VueConfiguration} */
	vueConfig

	initialized = false

	useModule(module) {

		ModuleRegistry.add(module);
		return this;
	}

	/**
	 * @param {string} el
	 */
	async mountAsync(el) {
		
		if (!this.initialized) {
			for (let module of ModuleRegistry.getAll()) {

				if (module.onInitAsync) {
					await module.onInitAsync(this.vueApp);
				}
			}
			this.initialized = true;
		}
		this.vueApp.mount(el);
	}
}

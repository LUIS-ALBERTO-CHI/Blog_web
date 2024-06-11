export default class AbstractModule {
	constructor() {
		if (this.constructor === AbstractModule)
			throw new TypeError('Abstract class "AbstractModule" cannot be instantiated directly');
	}

	/** @param {import('vue').App} vueApp
	 *  @param { any } options */
	onInitAsync(vueApp, options) {
		
	}

	/** @param {import('vue').default } vueInstance */
	onApplicationCreated(vueInstance) {

	}

	/** @param {String} menuType
	 *  @returns {
		 Promise<Array<{
			textKey: String,
			text: String,
			path: import('vue-router').RawLocation,
			icon: String
			}>>
	} */
	getMenuItemsAsync(menuType) {
		return [];//TODO: Utiliser un Enumerator ? https://dev.azure.com/fwaeu/TemplateCore/_workitems/edit/4602/
	}
}
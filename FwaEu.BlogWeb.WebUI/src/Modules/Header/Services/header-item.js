export class HeaderItem {

	/**@typedef {() => Promise<import("vue").VueConstructor>} ComponentFactory
	 * @param {{
		 * component: ComponentFactory, 
		 * smallModeContentComponent: ComponentFactory,
		 * fetchDataAsync: () => Promise }} configuration
	 * @param {Boolean} isVisible
	 */
	constructor(
		configuration,
		isVisible) {
		this.configuration = configuration;
		this.isVisible = isVisible;
		this.fetchedData = null;
	}
}
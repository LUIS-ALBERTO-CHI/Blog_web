export default class HeaderContext {
	/**
	 * @param {import("vue-router").default} router
	 * @param {import("vue-i18n").default} i18n
	 * @param {import("vue").Component} component
	 */
	constructor(router, i18n, component) {
		this.router = router;
		this.i18n = i18n;
		this.component = component;
	}
}
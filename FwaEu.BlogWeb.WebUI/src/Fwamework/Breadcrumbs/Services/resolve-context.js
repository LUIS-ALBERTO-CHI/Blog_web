export class BreadcrumbNode {
	/** @type String */
	text

	/** @type String */
	to

	/** @type BreadcrumbNode */
	parentNode
}

export class ResolveContext {
	/**@param {import("vue-router").default} router
	 * @param {import("vue-i18n").default} i18n
	 */
	constructor(router, i18n) {
		
		this.router = router;
		this.i18n = i18n;
		/** @type Array<BreadcrumbNode> */
		this.resolvedNodes = [];
	}
}

export default ResolveContext;
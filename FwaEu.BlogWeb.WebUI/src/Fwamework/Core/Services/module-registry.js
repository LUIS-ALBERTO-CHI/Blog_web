/**
* @type { Array<import('./abstract-module-class').default> }
*/
let modulesRegistry = [];

export default {
	getAll() {
		return modulesRegistry;
	},

	/**
	 * @param { import('./abstract-module-class').default } module
	 */
	add(module) {
		modulesRegistry.push(module);
	}
}

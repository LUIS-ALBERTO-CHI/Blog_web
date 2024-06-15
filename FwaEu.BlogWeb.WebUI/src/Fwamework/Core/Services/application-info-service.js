/**
* @typedef { { version: String, name: String, copyrightYears: String} } ApplicationInfo
 * @type ApplicationInfo
**/
let information = null;

/**
 * @typedef { { getAsync: Promise<ApplicationInfo> } } ApplicationInfoProvider
 * @type ApplicationInfoProvider
 * */
let provider = null;

export default {

	/**
	 * @param {ApplicationInfoProvider} applicationInfoProvider
	 */
	async configureAsync(applicationInfoProvider) {
		provider = applicationInfoProvider;
		await this.reloadAsync();
	},

	get() {
		return information;
	},

	async reloadAsync() {
		//NOTE: Retrieving the application information is not critical for application loading
		information = await provider.getAsync().catch(() => { return { version: 0, name: "", copyrightYears: null};});
	}
}
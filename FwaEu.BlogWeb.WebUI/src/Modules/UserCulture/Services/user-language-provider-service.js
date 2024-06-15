export default {
	/*Note: 1 is order stating that the language returned by this service is given preference over browser and default language provided 
	by  'browser-language-provider-service.js' and 'default-langauge-provider-service' service respectively*/
	priority: 1,
	async configureAsync() {
		const CurrentUserService = (await import("@/Fwamework/Users/Services/current-user-service")).default;

		CurrentUserService.onChanged(this.onCurrentUserChangedAsync.bind(this));
	},

	async getCurrentLanguageAsync() {
		const CurrentUserService = await import('@/Fwamework/Users/Services/current-user-service');
		let userDetails = await CurrentUserService.default.getAsync().catch(e => {
			console.warn("Unable to load current user culture" + e.toString());
		});
		return userDetails?.parts?.cultureSettings?.languageTwoLetterIsoCode;
	},

	async onCurrentUserChangedAsync() {
		const localizationService = (await import('@/Fwamework/Culture/Services/localization-service')).default;

		await localizationService.loadCurrentLanguageAsync();
		await localizationService.getGlobalMessagesAsync();
	}
}
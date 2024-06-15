
export default {
	/*Note: 2 is order stating that before loading the language returned by this service,
	the language provided by the 'user-language-provider-service.js' is given the priority*/
	priority: 2,
	getCurrentLanguageAsync() {
		let languageTwoLettersIsoCode = navigator.language.split('-', 1)[0];
		return languageTwoLettersIsoCode;
	}
}
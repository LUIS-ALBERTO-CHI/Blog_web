import { LocalStorage } from "@/Fwamework/Storage/Services/local-storage-store";

export default {
	//Note: 0 states that the language returned by this service is given the preference over user, browser and default language 
	priority: 0,
	getCurrentLanguageAsync() {
		return LocalStorage.getValue('LocalizationSampleForcedLanguage');
	}
}
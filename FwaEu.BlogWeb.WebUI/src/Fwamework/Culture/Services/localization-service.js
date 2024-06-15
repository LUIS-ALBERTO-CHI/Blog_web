import { createI18n, useI18n } from 'vue-i18n';
import DefaultNumberFormats from '@/Fwamework/Culture/Content/default-number-formats.json';
import DefaultDateTimeFormats from '@/Fwamework/Culture/Content/default-date-time-formats.json';
import CultureConfigurationProvider from '@/Fwamework/Culture/Services/language-configuration-provider-service';
import AsyncEventEmitter from '@/Fwamework/Core/Services/event-emitter-class';

const currentLanguageChangedEventEmitter = new AsyncEventEmitter();
const globalMessagesContext = import.meta.glob('/**/*-global-messages.*.json');
const defaultCulture = CultureConfigurationProvider.getCultureConfiguration().getDefaultCulture();

const i18nPlugin = createI18n({
	locale: defaultCulture.code,
	fallbackLocale: defaultCulture.code,
	messages: "",
	numberFormats: DefaultNumberFormats,
	datetimeFormats: DefaultDateTimeFormats,
	fallbackWarn: false,
	missingWarn: false,
	legacy: false,
	getMissingHandler: (locale, key, vm, values) => {
		return `[missing:${key}]`
	}
});
const i18n = i18nPlugin.global;
const loadedGlobalMessageLanguages = [];

export default {
	async configureAsync(vueApp) {
		vueApp.use(i18nPlugin);
		vueApp.mixin({
			beforeCreate() {
				this.$i18n = useI18n({ useScope: 'local'});
				this.$t = this.$i18n.t;
				this.$d = this.$i18n.d;
				this.$te = this.$i18n.te;
				this.$tm = this.$i18n.tm;
				this.$n = this.$i18n.n;
			}
		})
	},

	onCurrentLanguageChanged(listener) {
		return currentLanguageChangedEventEmitter.addListener(listener);
	},

	async setCurrentLanguageAsync(language) {
		const languageHasChanged = i18n.locale.value != language;
		if (languageHasChanged) {
			//NOTE: the language change must be saved on data base. Here we only want to reaload the base in order to download the new translations
			await currentLanguageChangedEventEmitter.emitAsync(language);
			setTimeout(()=> window.location.reload(), 500);//Delay the reload in order to allow current actions to be finished
		}
	},

	getCurrentLanguage() {
		return i18n.locale.value;
	},

	async loadCurrentLanguageAsync() {
		const supportedLanguagesCode = this.getSupportedLanguagesCode();
		let languageToSet = defaultCulture.code;

		let currentLanguageProviders = (await import('@/Fwamework/Culture/Services/language-providers-service')).default
			.getAll().sort((a, b) => a.priority - b.priority);

		for (let provider of currentLanguageProviders) {
			try {
				let currentLanguage = await provider.getCurrentLanguageAsync();
				if (currentLanguage && supportedLanguagesCode.includes(currentLanguage)) {
					languageToSet = currentLanguage;
					break;
				}
			} catch {
				//NOTE: Some language providers may fail but it is not critical
			}
		}

		i18n.locale.value = languageToSet;
	},

	getDefaultLanguageCode() {
		return defaultCulture.code;
	},

	getSupportedLanguagesCode() {
		return this.getSupportedLanguages().map(x => x.code);
	},

	getSupportedLanguages() {		
		return CultureConfigurationProvider.getCultureConfiguration().getAllCultures();
	},

	/**
	 * Lazy load of module global messages for current language
	 * */
	async getGlobalMessagesAsync() {

		const currentLanguage = this.getCurrentLanguage();
		if (loadedGlobalMessageLanguages.includes(currentLanguage))
			return;
		
		const globalMessagesPromises = Object.keys(globalMessagesContext).filter(path => path.endsWith("." + currentLanguage + ".json"))
			.map(path => globalMessagesContext[path]());
		
		await this.loadGlobalMessagesAsync(globalMessagesPromises, currentLanguage);
		loadedGlobalMessageLanguages.push(currentLanguage);
	},

	async loadGlobalMessagesAsync(globalMessagesPromises, locale = null) {
		let loadedMessagesResult = await Promise.all(globalMessagesPromises);
		for (let messagesResult of loadedMessagesResult) {
			i18n.mergeLocaleMessage(locale || i18n.locale.value, messagesResult);
		}
	}
}

export const I18n = i18n;

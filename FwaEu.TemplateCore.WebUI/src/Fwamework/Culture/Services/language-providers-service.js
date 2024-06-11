const context = import.meta.glob('/**/*-language-provider-service.js', { import: 'default', eager: true });

/**
 * @typedef {{ getCurrentLanguageAsync: () => Promise<String> }} LanguageProvider */

/** @type {{getValues: () => Array<LanguageProvider> }} */


export default {
	getAll() {
		return Object.values(context);
	}
};
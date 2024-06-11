const context = import.meta.glob('/**/culture-configuration-provider.js', { import: 'default', eager: true });

/**
 * @typedef {{code: String, imageSrc: any }} CultureItem
 * @typedef {{ getAllCultures: () => Array<CultureItem>, getDefaultCulture: () => CultureItem }} CultureConfiguration */

/** @type {{getValues: () => Array<CultureConfiguration> }} */


export default {

	getCultureConfiguration() {
		return Object.values(context)[0];
	}
};
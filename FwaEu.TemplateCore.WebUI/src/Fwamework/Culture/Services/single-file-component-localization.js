/**
 * @param {import("vue/types/vue").Vue} componentInstance
 * @param {Record<String, () => Promise>} loadMessagesImportGlob Ex:  Visit https://vitejs.dev/guide/features.html#glob-import
 */
export async function loadMessagesAsync(componentInstance, loadMessagesImportGlob) {
	const loadMessagePromise = Object.entries(loadMessagesImportGlob)
		.find(entry => entry[0].endsWith("." + componentInstance.$i18n.locale.value + ".json"))[1];
	const messagesResult = await loadMessagePromise();

	componentInstance.$i18n.mergeLocaleMessage(componentInstance.$i18n.locale.value, messagesResult.default);
}

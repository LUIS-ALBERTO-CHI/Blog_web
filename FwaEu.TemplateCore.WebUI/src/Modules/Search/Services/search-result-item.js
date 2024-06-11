export class SearchResultItem {

	/**@param {String} icon
	 * @param {String} text
	 * @param {String} description
	 * @param {import("vue-router").Location} link
	 */
	constructor(icon, text, description, link) {
		this.icon = icon;
		this.text = text;
		this.description = description;
		this.link = link;
	}

	/** @type {String} */
	icon;

	/** @type {String} */
	text;

	/** @type {String} */
	description;

	/** @type {import("vue-router").Location} */
	link;
}
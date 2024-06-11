export default class PersistentNotificationModel {

	/** @type {String} */
	id;

	/** @type {String}  */
	notificationType;

	/** @type {Date} */
	sentOn;

	/** @type {String}  */
	seenOn;

	/** @type {String} */
	model;

	/** @type {Boolean} */
	isSticky;

	constructor(id, notificationType, sentOn, seenOn, model, isSticky)
	{
		this.id = id;
		this.notificationType = notificationType;
		this.sentOn = new Date(sentOn);
		this.seenOn = seenOn;
		this.model = model;
		this.isSticky = isSticky;
	}
}
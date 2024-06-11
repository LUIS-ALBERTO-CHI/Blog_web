import { I18n } from "@/Fwamework/Culture/Services/localization-service";

function toDate(value)
{
	return typeof value === 'string'
		? new Date(value)
		: value;
}

export default {
	createModel(text, date, userId)
	{
		return {
			text: text,
			data: {
				date: date,
				userId: userId
			}
		}
	},
	createCreatedAndUpdatedModels(creationInfo, updateInfo)
	{
		return [
			{ info: creationInfo, resource: 'timelineCreation' },
			{ info: updateInfo, resource: 'timelineUpdate' }
			]
			.filter(item => item.info && item.info.date)
			.map(item => this.createModel(
				I18n.t(item.resource),
				toDate(item.info.date),
				item.info.userId));
	}
}
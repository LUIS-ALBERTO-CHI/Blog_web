import { I18n } from "@/Fwamework/Culture/Services/localization-service";
import TimelineService from '@/Fwamework/Users/Services/timeline-service';
import { defineAsyncComponent } from "vue";

export default {
	partName: "history",
	component: defineAsyncComponent(() => import("@/Modules/UserHistory/Components/UserHistoryPartComponent.vue")),
	initializeAsync: function (user, context)
	{
		let userPart = user.parts[this.partName];
		if (!userPart)
			return [];

		const timelineModels = TimelineService.createCreatedAndUpdatedModels(
			{ date: userPart.createdOn, userId: userPart.createdById },
			{ date: userPart.updatedOn, userId: userPart.updatedById });
			
		return [{
			data: timelineModels,
			title: I18n.t('historyTitle'),
			location: 'timeline'
		}];
	}
}

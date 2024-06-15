import { UserTask } from "@/Modules/UserTasks/Services/user-task";
import { ListDisplayZoneName } from "@/Modules/UserTasksList/Services/user-task-display-zone-handler";
import { I18n } from "@/Fwamework/Culture/Services/localization-service";
import { NavigationMenuDisplayZoneName } from "@/Modules/UserTasksNavigationMenu/Services/user-task-display-zone-handler";

export const FarmsWitoutAnimalsTaskName = "FarmsWithoutAnimals";

export class FarmsWitoutAnimalsUserTask extends UserTask {

	constructor() {
		super(FarmsWitoutAnimalsTaskName, [ListDisplayZoneName, NavigationMenuDisplayZoneName]);
	}

	async getParametersAsync() {
		const router = (await import('@/Fwamework/Routing/Services/vue-router-service')).default;
		const activityId = router.currentRoute.value.query.activityId ?? null;
		return { activityId };
	}

	createDisplayZoneModel(displayZoneName, value) {
		if (displayZoneName === ListDisplayZoneName) {
			return {
				title: I18n.t("farmsWithoutAnimalsUserTaskTitle"),
				value: value.count,
				path: { name: 'FarmsWithoutAnimals' }
			};
		}
		else {
			return value;
		}
	}
}

export default new FarmsWitoutAnimalsUserTask();
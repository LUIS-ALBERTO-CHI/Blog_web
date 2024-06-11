import { UserTask } from "@/Modules/UserTasks/Services/user-task";
import { ListDisplayZoneName } from "@/Modules/UserTasksList/Services/user-task-display-zone-handler";
import { I18n } from "@/Fwamework/Culture/Services/localization-service";

export const FarmerCountTaskName = "FarmerCount";

export class FarmerCountUserTask extends UserTask {

	constructor() {
		super(FarmerCountTaskName, [ListDisplayZoneName]);
	}

	createDisplayZoneModel(displayZoneName, value) {
		return {
			title: I18n.t("farmersUserTaskTitle"),
			value: value.count
		};
	}
}
export default new FarmerCountUserTask();
import { UserTaskDisplayZoneHandler } from "@/Modules/UserTasks/Services/user-task-display-zone-handler";
import NavigationMenuService from "@/Fwamework/NavigationMenu/Services/navigation-menu-service";

export const NavigationMenuDisplayZoneName = "navigationMenu";

/** @typedef {import("@/Modules/UserTasks/Services/user-task").UserTask} UserTask 
	@typedef {import( "./menu-counter-user-task-item").MenuCounterUserTaskItem} MenuCounterUserTaskItem
 */

export class UserTaskNavigationMenuDisplayZoneHandler extends UserTaskDisplayZoneHandler {
	constructor() {
		super(NavigationMenuDisplayZoneName);
	}

	/** @param {UserTask} userTask @returns {Promise}*/
	async handleUserTaskUpdateAsync(userTask) {
		const allMenuItems = await NavigationMenuService.getMenuItemsAsync();
		const menuItem = allMenuItems.filter(x => x.userTask?.name === userTask.name);

		await this.updateMenuItemAsync(menuItem, userTask);
		await NavigationMenuService.setMenuItemsAsync(allMenuItems);
	}

	/**@param {{userTask: MenuCounterUserTaskItem }} menuItem
	 * @param {UserTask} userTask
	 * @returns {Promise}
	 */
	async updateMenuItemAsync(menuItem, userTask) {
		const newUserTaskValue = await userTask.getValueAsync(this.zoneName);
		menuItem.userTask.setValue(newUserTaskValue);
	}
}

export default new UserTaskNavigationMenuDisplayZoneHandler();

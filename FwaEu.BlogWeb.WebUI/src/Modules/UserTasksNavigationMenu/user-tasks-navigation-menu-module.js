import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";
import UserTasksManagerService from "@/Modules/UserTasks/Services/user-tasks-manager-service";
import UserTaskNavigationMenuDisplayZoneHandler from "./Services/user-task-display-zone-handler";
import NavigationMenuService from "@/Fwamework/NavigationMenu/Services/navigation-menu-service";

export class UserTasksNavigationMenuModule extends AbstractModule {

	async onInitAsync() {
		UserTasksManagerService.addDisplayZoneHandler(UserTaskNavigationMenuDisplayZoneHandler);
		this._navigationMenuMountedEventOff = NavigationMenuService.onMounted(this.onNavigationMenuMountedAsync.bind(this));
	}

	async onNavigationMenuMountedAsync() {
		const allTasks = await UserTaskNavigationMenuDisplayZoneHandler.getAllTasksAsync();
		const allMenuItems = await NavigationMenuService.getMenuItemsAsync();

		const rawFullMenuItems = allMenuItems.flatMap(mi => [mi].concat(mi.items ?? []));

		const tasksWithMenuItems = rawFullMenuItems.map(mi => {
			const taskWithMenuItem = {
				userTask: allTasks.find(t => t.name === mi.userTask?.name),
				menuItem: mi
			};
			return taskWithMenuItem;
		}).filter(tmi => tmi.userTask);


		const handleUpdatePromises = tasksWithMenuItems.map(tmi => UserTaskNavigationMenuDisplayZoneHandler.updateMenuItemAsync(tmi.menuItem, tmi.userTask));
		await Promise.all(handleUpdatePromises);
		await NavigationMenuService.setMenuItemsAsync(allMenuItems);
	}
}
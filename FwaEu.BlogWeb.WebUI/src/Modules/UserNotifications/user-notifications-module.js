import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";

export class UserNotificationsModule extends AbstractModule
{
	/**
	* @param { {signalRAdapter:import("@/Modules/UserNotifications/Services/user-notification-service").SignalRAdapter } options
    */
	constructor(options) {
		super();
		this.options = options;
	}
	async onInitAsync(vueApp)
	{
		const UserNotificationService = (await import("@/Modules/UserNotifications/Services/user-notification-service")).default;
		await UserNotificationService.initAsync(this.options.signalRAdapter);
	}
}
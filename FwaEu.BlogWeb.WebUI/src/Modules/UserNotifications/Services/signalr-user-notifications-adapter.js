import { Configuration } from "@/Fwamework/Core/Services/configuration-service";
import AuthenticationService from "@/Fwamework/Authentication/Services/authentication-service";
import { HubConnectionBuilder } from "@microsoft/signalr";
import LocalizationService from '@/Fwamework/Culture/Services/localization-service';


export default class SignalRUserNotificationsAdapter {

	/**
	* @type {import("@microsoft/signalr").HubConnection}
	*/
	_connection = null;

	async disconnectAsync() {
		if (this._connection != null) {
			await this._connection.stop();
			this._connection = null;
		}
	}

	/**
	 * @param {import("./user-notification-service").OnNotifiedAsync} onNotifiedAsync 
	 * @param {import("./user-notification-service").OnStartAsync} onStartAsync 
	 * @param {import("./user-notification-service").OnStopAsync} onStopAsync 
	 */
	async connectAsync(onNotifiedAsync, onStartAsync, onStopAsync) {
		const currentLanguage = LocalizationService.getCurrentLanguage();

		const connection = new HubConnectionBuilder()
			.withUrl(Configuration.fwamework.core.apiEndpoint + "UserNotifications?culture=" + currentLanguage, {
				accessTokenFactory: AuthenticationService.getCurrentTokenAsync
			})
			.withAutomaticReconnect()
			.build();

		connection.onclose(async function (error) {
			await onStopAsync({ error });
		});

		connection.on("Notified", async function (notificationType, content) {
			await onNotifiedAsync(notificationType, content);
		});


		await connection.start();
		this._connection = connection;
		await onStartAsync({ connectionId: connection.connectionId });
	}
}
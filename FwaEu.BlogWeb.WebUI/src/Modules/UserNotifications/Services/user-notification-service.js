import AuthenticationService from "@/Fwamework/Authentication/Services/authentication-service";
import AsyncEventEmitter from "@/Fwamework/Core/Services/event-emitter-class";


/**
 * @typedef {(onNotifiedAsync:OnNotifiedAsync, onStartAsync:OnStartAsync, onStopAsync:OnStopAsync)=> Promise} ConnectAsync
 * @typedef {()=> Promise} DisconnectAsync 
 * @typedef {(notificationType:String, content:any) => Promise} OnNotifiedAsync
 * @typedef {(connectionId:String)=> Promise} OnStartAsync
 * @typedef {(error:Error)=> Promise} OnStopAsync
 * @typedef {{connectAsync:ConnectAsync, disconnectAsync: DisconnectAsync }} SignalRAdapter
 * */

class UserNotificationService {

	/** @param {SignalRAdapter} signalRAdapter */
	async initAsync(signalRAdapter) {		
		this._signalRAdapter = signalRAdapter;
		this._connectionStartEventEmitter = new AsyncEventEmitter(true);
		this._connectionErrorEventEmitter = new AsyncEventEmitter(true);
		this._connectionStopEventEmitter = new AsyncEventEmitter(true);
		this._notifiedEventEmitter = new AsyncEventEmitter(true);

		AuthenticationService.onLoggedIn(this.startConnectionAsync.bind(this));
		AuthenticationService.onLoggedOut(this.stopConnectionAsync.bind(this));

		//If the user is already authenticated then initialize the connection, otherwise the connection will be started at onLoggedIn event
		if (await AuthenticationService.isAuthenticatedAsync())
			await this.startConnectionAsync();
	}

	isConnected = false;

	async _disposeConnectionAsync() {
		if (this.isConnected != false) {
			await this._signalRAdapter.disconnectAsync();
			this.isConnected = false;
		}
	}

	getConnectionState() {
		return this._connection?.state;
	}

	async startConnectionAsync() {
		try {
			await this.stopConnectionAsync();
			const $this = this;
			await this._signalRAdapter.connectAsync(
				async (notificationType, content) => await $this._notifiedEventEmitter.emitAsync({ notificationType, content }),
				async () => { $this.isConnected = true; await $this._connectionStartEventEmitter.emitAsync({ connectionId: $this._connection?.id }); },
				async (error) => { $this.isConnected = false; await $this._connectionStopEventEmitter.emitAsync({ error });}
			);
		}
		catch (error) {
			await this._connectionErrorEventEmitter.emitAsync({ connectionId: this._connection?.id, error });
		}
	}

	async stopConnectionAsync() {
		await this._disposeConnectionAsync();
	}

	/**@param {(e: {connectionId: String }) => Promise} listener @returns {() => void} */
	onStarted(listener) {
		return this._connectionStartEventEmitter.addListener(listener);
	}

	/**@param {(e: {notificationType: String, model: any }) => Promise} listener @returns {() => void} */
	onNotified(listener) {
		return this._notifiedEventEmitter.addListener(listener);
	}

	/**@param {(e: {error: Error }) => Promise} listener @returns {() => void} */
	onStopped(listener) {
		return this._connectionStopEventEmitter.addListener(listener);
	}

	/**@param {(e: {connectionId: String, error: Error }) => Promise} listener @returns {() => void} */
	onError(listener) {
		return this._connectionErrorEventEmitter.addListener(listener);
	}
}

export default new UserNotificationService();
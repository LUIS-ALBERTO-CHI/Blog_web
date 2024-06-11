import AsyncEventEmitter from "@/Fwamework/Core/Services/event-emitter-class";
import AsyncQueue from "@/Fwamework/Concurrency/async-queue";

const changedEventEmitter = new AsyncEventEmitter();
const getCurrentUserQueue = new AsyncQueue(1, true);//Use an async queue in order to control concurrent calls
const currentUserStoreKey = "currentUser";

/** @type {import("@/Fwamework/Storage/Services/abstract-store").default} */
let cacheStore;

export default {

	/** @param {({currentUser: any}) => Promise} listener */
	onChanged(listener) {
		return changedEventEmitter.addListener(listener);
	},

	async configureAsync(options) {
		cacheStore = options?.cacheStore ?? (await import("@/Fwamework/Routing/Services/routing-cache-store")).default;
		const AuthenticationService = (await import("@/Fwamework/Authentication/Services/authentication-service")).default;
		AuthenticationService.onLoggedIn(this.onAuthenticationStateChangeAsync.bind(this));
		AuthenticationService.onLoggedOut(this.onAuthenticationStateChangeAsync.bind(this));
	},

	async getAsync(forceReload = false) {
		return await getCurrentUserQueue.runAsync(async () => {

			const authenticationService = (await import("@/Fwamework/Authentication/Services/authentication-service")).default;

			let isAuthenticated = await authenticationService.isAuthenticatedAsync();
			if (isAuthenticated) {
				if (!await cacheStore.getValueAsync(currentUserStoreKey) || forceReload) {
					const httpService = (await import('@/Fwamework/Core/Services/http-service')).default;

					let response = await httpService.get(`Users/current`);
					await cacheStore.setValueAsync(currentUserStoreKey, response.data);
				}
				return await cacheStore.getValueAsync(currentUserStoreKey);
			}
			return null;
		});
	},

	async onAuthenticationStateChangeAsync() {
		const eventArgs = {
			currentUser: await this.getAsync(true)
		};
		await changedEventEmitter.emitAsync(eventArgs);
	}
}
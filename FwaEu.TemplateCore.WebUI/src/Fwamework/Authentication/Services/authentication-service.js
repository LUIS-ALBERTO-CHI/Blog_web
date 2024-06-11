import AsyncEventEmitter from '@/Fwamework/Core/Services/event-emitter-class';

const loggedInEventEmitter = new AsyncEventEmitter();
const loggedOutEventEmitter = new AsyncEventEmitter();

/** @type { AuthenticationHandler } */
let authenticationHandlers = null;
let disableListeners = false;
export function setEnableListeners(enabled) {
	disableListeners = !enabled;
}
export default {

	/** @param {() => Promise} listener */
	onLoggedIn(listener) {
		return loggedInEventEmitter.addListener(listener);
	},

	/** @param {() => Promise} listener */
	onLoggedOut(listener) {
		return loggedOutEventEmitter.addListener(listener);
	},

	setAuthenticationHandlers(handler) {
		authenticationHandlers = handler;
	},

	getAuthenticationHandlers() {
		return authenticationHandlers;
	},

	async createLoginComponentAsync() {
		return await Promise.all(authenticationHandlers.filter(ah => ah.createLoginComponentAsync !== null).map(ah => 
			ah.createLoginComponentAsync().then(c => ({ 'loginComponent': c, 'handlerKey': ah.handlerKey }))
		));
	},

	async getCurrentTokenAsync() {
		for (const handler of authenticationHandlers) {
			if (await handler.isAuthenticatedAsync()) {
				return await handler.getCurrentTokenAsync();
			}
		}
		return null;
	},

	async renewTokenAsync() {
		for (const handler of authenticationHandlers) {
			if (await handler.isAuthenticatedAsync()) {
				return await handler.renewTokenAsync();
			}
		}
		return null;
	},

	async loginAsync(handlerKey, request) {
		const handler = authenticationHandlers.find(ah => ah.handlerKey === handlerKey);

		if (!handler) {
			throw new Error(`No authentication handler matchs this handler key '${handlerKey}'`);
		}
		const result = await handler.loginAsync(request);

		if (result && !disableListeners)
			await loggedInEventEmitter.emitAsync();
		return result;
	},

	async isAuthenticatedAsync() {
		for (const handler of authenticationHandlers) {
			if (await handler.isAuthenticatedAsync()) {
				return true;
			}
		}
		return false;
	},

	async logoutAsync() {
		let success = true;
		for (const handler of authenticationHandlers) {
			if (await handler.isAuthenticatedAsync()) {
				success = success && await handler.logoutAsync();

			}
		}
		if (success && !disableListeners) {
			await loggedOutEventEmitter.emitAsync();
		}
		return success;
	}
}

export class AuthenticationHandler {

	constructor(handlerKey) {
		if (!handlerKey) {
			throw new Error('You must provide a handlerKey.');
		}
		this.handlerKey = handlerKey;
	}

	/** @type {()=> Promise<import('vue').VueConstructor> | null}  **/
	createLoginComponentAsync = null;

	async configureAsync() {
		throw new Error('You must implement configureAsync');
	}

	async loginAsync(request) {
		throw new Error('You must implement loginAsync');
	}

	async getCurrentTokenAsync() {
		throw new Error('You must implement getCurrentTokenAsync');
	}

	async isAuthenticatedAsync() {
		throw new Error('You must implement isAuthenticatedAsync');
	}

	async logoutAsync() {
		throw new Error('You must implement logoutAsync');
	}
}
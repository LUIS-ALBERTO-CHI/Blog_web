import HttpService from '@/Fwamework/Core/Services/http-service';
import { jwtDecode } from 'jwt-decode';
import LocalStorageStore from '@/Fwamework/Storage/Services/local-storage-store';
import CacheManager from '@/Fwamework/Cache/Services/cache-manager';
import { Configuration } from "@/Fwamework/Core/Services/configuration-service";
import { AuthenticationHandler } from '@/Fwamework/Authentication/Services/authentication-service';
import { defineAsyncComponent } from 'vue';

const cacheManager = new CacheManager(new LocalStorageStore());
export const AuthenticationHandlerKey = 'Default';
export class DefaultAuthenticationHandler extends AuthenticationHandler {

	constructor(handlerKey = AuthenticationHandlerKey, currentUserTokenCacheKey = 'CurrentUserToken') {
		super(handlerKey);
		this.currentUserTokenCacheKey = currentUserTokenCacheKey;
	}

	createLoginComponentAsync = async function() {
		return defineAsyncComponent(() => import('@UILibrary/Modules/DefaultAuthentication/Components/LoginFormComponent.vue'));
	}

	async configureAsync() {
		if (await this.isAuthenticatedAsync()) {
			const existingToken = await this.getCurrentTokenAsync();
			if (existingToken) {
				await this.configureTokenCacheAsync(existingToken);
			}
		}
	}

	getCurrentTokenAsync() {
		return cacheManager.getItemValueAsync(this.currentUserTokenCacheKey);
	}

	async loginAsync(request) {
		const { identity, password } = request;
		let response = await HttpService.post('Authentication/Authenticate', { identity: identity, password: password });

		let token = response.data.token;

		await this.authenticateWithTokenAsync(token);

		return response.data;
	}

	async authenticateWithTokenAsync(token) {

		//If cache is not configured yet, then do it
		if (!cacheManager.getStatus(this.currentUserTokenCacheKey))
			await this.configureTokenCacheAsync(token);

		await cacheManager.updateValue(this.currentUserTokenCacheKey, token).saveChangesAsync();
	}

	async renewTokenAsync(token) {
		if (!token)
			token = await this.getCurrentTokenAsync();
		return await HttpService.post('Authentication/RenewToken', null, { headers: { Authorization: `Bearer ${token}` } })
			.then(response => response.data);
	}

	async configureTokenCacheAsync(token) {
		const $this = this;
		var options = {
			expirationDelayInSeconds: this.getRemainingTimeBeforeExpirationInSeconds(token),
			expirationCallbackAsync: (cacheItem) => { return $this.renewCurrentTokenWithRetryAsync(cacheItem); }
		};

		await cacheManager.configureItemAsync(this.currentUserTokenCacheKey, options);
	}

	async renewCurrentTokenWithRetryAsync(cacheItem) {

		let trialNumber = Configuration.fwamework.authentication.maxTokenRenewalAttempts;
		let token = await this.getCurrentTokenAsync();

		for (var i = 0; i < trialNumber; i++) {
			let data = await this.renewTokenAsync(token);
			let newToken = data.token;
			if (newToken) {
				cacheItem.options.expirationDelayInSeconds = this.getRemainingTimeBeforeExpirationInSeconds(newToken);
				await cacheManager.updateValue(this.currentUserTokenCacheKey, newToken).saveChangesAsync();
				break;
			}
		}
	}

	getExpirationDate(token) {
		let jsonValue = jwtDecode(token);
		return new Date(jsonValue.exp * 1000);
	}

	getRemainingTimeBeforeExpirationInSeconds(token) {
		let expirationdate = this.getExpirationDate(token);
		let remainingTime = expirationdate - new Date();
		return (remainingTime / 1000);
	}

	async isAuthenticatedAsync() {
		const token = await this.getCurrentTokenAsync();
		return token && this.getExpirationDate(token) > new Date();
	}

	async logoutAsync() {
		await cacheManager.removeItemAsync(this.currentUserTokenCacheKey);
		return true;
	}
}

export default new DefaultAuthenticationHandler();
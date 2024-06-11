import { LocalStorage } from '@/Fwamework/Storage/Services/local-storage-store';
import HttpService from '@/Fwamework/Core/Services/http-service';
import { Configuration } from "@/Fwamework/Core/Services/configuration-service";

const currentUserTokenCacheKey = 'SetupCurrentUserToken';

export default {

	isAuthenticationEnabled() {
		return !(Configuration.fwamework.setup?.disableAuthentication)
	},

	getCurrentToken() {
		return LocalStorage.getValue(currentUserTokenCacheKey);
	},

	async loginAsync(user, password) {
		let response = await HttpService.post('Setup/Authenticate', { login: user, password: password });
		LocalStorage.setValue(currentUserTokenCacheKey, response.data.token);

		return response.data;
	},

	isAuthenticated() {
		const token = this.getCurrentToken();
		return token;
	},

	logout() {
		LocalStorage.removeValue(currentUserTokenCacheKey);
	}
}
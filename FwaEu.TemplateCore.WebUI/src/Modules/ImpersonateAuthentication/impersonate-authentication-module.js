import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';
import SetupImpersonateAuthenticationHandler from './Setup/setup-impersonate-authentication-handler';

export class ImpersonateAuthenticationModule extends AbstractModule {

	async onInitAsync() {
		await SetupImpersonateAuthenticationHandler.configureAsync();
	}
}
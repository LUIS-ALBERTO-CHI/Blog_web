import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';
import AzureADAuthenticationHandler from './Services/azure-ad-authentication-handler';


export class AzureADAuthenticationModule extends AbstractModule {
	
	async onInitAsync() {
		await AzureADAuthenticationHandler.configureAsync();
	}
}

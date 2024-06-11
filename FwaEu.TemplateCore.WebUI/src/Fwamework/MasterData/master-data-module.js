import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';
import AuthenticationService from '@/Fwamework/Authentication/Services/authentication-service';
import MasterDataManagerService from './Services/master-data-manager-service';
import MasterDataRoutingService from './Services/master-data-routing-service';

export class MasterDataModule extends AbstractModule {

	constructor(options) {
		super();
		if (!options?.defaultStore)
			throw new Error('Missing required option "defaultStore", you must provide a default master data store');
		this.options = options;

	}

	async onInitAsync(vueApp) {

		
		MasterDataManagerService._defaultStore = this.options.defaultStore;
		await MasterDataRoutingService.configureAsync(vueApp);

		AuthenticationService.onLoggedOut(async () => await MasterDataManagerService.clearCacheAsync());
	}
}

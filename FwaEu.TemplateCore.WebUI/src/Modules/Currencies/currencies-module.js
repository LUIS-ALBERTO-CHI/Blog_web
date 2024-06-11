import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";
import CurrenciesMasterDataService from '@/Modules/Currencies/Services/currency-master-data-service';
import GenericAdminConfigurationService from '@/Modules/GenericAdmin/Services/generic-admin-configuration-service';
import CurrencyConfiguration from '@/Modules/Currencies/Services/currency-configuration'; 
import ExchangeRateConfiguration from '@/Modules/Currencies/Services/exchange-rate-configuration';

export class CurrenciesModule extends AbstractModule {

	async onInitAsync() {
		await CurrenciesMasterDataService.configureAsync();
		
		GenericAdminConfigurationService.register(CurrencyConfiguration);
		GenericAdminConfigurationService.register(ExchangeRateConfiguration);
	}
}

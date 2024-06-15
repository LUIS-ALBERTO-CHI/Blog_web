import AbstractConfiguration from '@/Modules/GenericAdmin/Services/abstract-configuration';
import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
import { CanAdministrateCurrencies } from '@/Modules/Currencies/currencies-permissions';
import { hasPermissionAsync } from '@/Fwamework/Permissions/Services/current-user-permissions-service';

class ExchangeRateConfiguration extends AbstractConfiguration {
	constructor() {
		super();

		this.columnsCustomizer.addCustomization('baseCurrencyCode', { index: 20, width: 120 });
		this.columnsCustomizer.addCustomization('quotedCurrencyCode', { index: 21, width: 150 });

		this.columnsCustomizer.addCustomization('date', { index: 30, width: 130 });
		this.columnsCustomizer.addCustomization('value', { index: 31 });
		this.columnsCustomizer.addCustomization('isInverse', { index: 2000, width: 100 });
	}

	async loadMessagesAsync(component) {
		await loadMessagesAsync(component, import.meta.glob('@/Modules/Currencies/Content/exchange-rate-common.*.json'));
    }

	getPageTitleKey() {
		return 'exchangeRatesPageTitle';
	}

	getDescriptionKey() {
		return 'exchangeRatesDescription';
	}

	getGroupTextKey() {
		return 'currenciesPageTitle';
	}
}

export default {
	configurationKey: 'ExchangeRateEntity',
	icon: "fas fa-exchange-alt",
	getConfiguration: function () {
		return new ExchangeRateConfiguration();
	},
	async isAccessibleAsync() {
		return await hasPermissionAsync(CanAdministrateCurrencies);
	}
};
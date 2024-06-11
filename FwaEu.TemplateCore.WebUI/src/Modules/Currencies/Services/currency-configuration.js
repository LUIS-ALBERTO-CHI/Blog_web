import AbstractConfiguration from '@/Modules/GenericAdmin/Services/abstract-configuration';
import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
import { hasPermissionAsync } from '@/Fwamework/Permissions/Services/current-user-permissions-service';
import { CanAdministrateCurrencies } from '@/Modules/Currencies/currencies-permissions';


class CurrencyConfiguration extends AbstractConfiguration {
	constructor() {
		super();

		this.columnsCustomizer.addCustomization('currencyCode', { index: 21, width: 120 });
		this.columnsCustomizer.addCustomization('isInverse', { index: 2000, width: 100 });
	}

	async loadMessagesAsync(component) {
		await loadMessagesAsync(component, import.meta.glob('@/Modules/Currencies/Content/currency-common.*.json'));
    }

	getPageTitleKey() {
		return 'currenciesPageTitle';
	}

	getGroupTextKey() {
		return 'currenciesPageTitle';
	}
}

export default {
	configurationKey: 'CurrencyEntity',
	icon: "fas fa-sack",
	getConfiguration: function () {
		return new CurrencyConfiguration();
	},
	async isAccessibleAsync() {
		return await hasPermissionAsync(CanAdministrateCurrencies);
	}
};
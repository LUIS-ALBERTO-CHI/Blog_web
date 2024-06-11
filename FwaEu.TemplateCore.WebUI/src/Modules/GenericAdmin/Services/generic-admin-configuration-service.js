import HttpService from '@/Fwamework/Core/Services/http-service';

let genericAdminConfigurationRegistry = [];

export default {
	getConfiguration(key, viewContext) {
		return HttpService.post('GenericAdmin/GetConfiguration/' + key, {
			value: viewContext
		}).then(response => {
			return response.data;
		});
	},
	getAll() {
		return genericAdminConfigurationRegistry;
	},
	get(key) {
		return genericAdminConfigurationRegistry.find(c => c.configurationKey === key);
	},
	register(genericAdminConfiguration) {
		genericAdminConfigurationRegistry.push(genericAdminConfiguration);
	}
};
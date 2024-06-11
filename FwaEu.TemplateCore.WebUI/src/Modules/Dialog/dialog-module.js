import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";
import DialogService from "@UILibrary/Modules/Dialog/Services/dialog-service";

export class DialogModule extends AbstractModule {
	onInitAsync(vueApp, options) {
		DialogService.initialize(vueApp);
	}

	onApplicationCreated(vueApp) {
		DialogService.configure(vueApp);
	}
}
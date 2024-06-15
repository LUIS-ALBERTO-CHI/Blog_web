import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";
import ApplicationInfoService from "@/Fwamework/Core/Services/application-info-service";
import ClickOutsideDirective from "@/Fwamework/Utils/Directives/click-outside-directive";
import { OnBeforeMount, OnBeforeUnmount, OnBeforeUpdate, OnCreated, OnMounted, OnUnmounted, OnUpdated } from "@/Fwamework/Utils/Directives/component-hooks-directives";

export class CoreModule extends AbstractModule {

	constructor(options) {
		super();
		if (!options?.applicationInfoProvider)
			throw new Error("Missing required option 'applicationInfoProvider'");
		this.options = options;
		
	}

	async onInitAsync(vueApp) {

		
		await ApplicationInfoService.configureAsync(this.options.applicationInfoProvider);

		vueApp.directive('click-outside', ClickOutsideDirective);
		vueApp.directive('created', OnCreated);
		vueApp.directive('before-mount', OnBeforeMount);
		vueApp.directive('mounted', OnMounted);
		vueApp.directive('before-update', OnBeforeUpdate);
		vueApp.directive('updated', OnUpdated);
		vueApp.directive('before-unmount', OnBeforeUnmount);
		vueApp.directive('unmounted', OnUnmounted);
	}
}

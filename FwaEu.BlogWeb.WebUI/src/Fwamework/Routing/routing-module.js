import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';
import Router from "@/Fwamework/Routing/Services/vue-router-service.js";
import RoutingCacheStore from '@/Fwamework/Routing/Services/routing-cache-store';
import RoutingErrorHandler from '@/Fwamework/Routing/Services/routing-error-handler';
import ErrorHandlerService from '@/Fwamework/Errors/Services/error-handler-service';

export class RoutingModule extends AbstractModule {

	constructor(options) {
		super();

		if (!options?.routerOptions?.routes) {
			throw new Error('Missing routes paramater');
		}
		for (let route of options.routerOptions.routes) {
			Router.addRoute(route);

		}
	}

	onInitAsync(vueApp) {
		vueApp.use(Router);
		RoutingCacheStore.configure(Router);
		ErrorHandlerService.registerErrorHandler(RoutingErrorHandler);
    }
}



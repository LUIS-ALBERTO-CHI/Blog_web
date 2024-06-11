import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';
import ErrorHandlerService from "@/Fwamework/Errors/Services/error-handler-service";

export class ErrorModule extends AbstractModule {
	onInitAsync(vueApp) {
		ErrorHandlerService.configure(vueApp);
	}
}

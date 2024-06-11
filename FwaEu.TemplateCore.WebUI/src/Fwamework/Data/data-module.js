import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';
import ErrorHandlerService from '@/Fwamework/Errors/Services/error-handler-service';
import DatabaseErrorHandlerService from '@/Fwamework/Data/Services/database-error-handler';

export class DataModule extends AbstractModule {
	async onInitAsync() {
		ErrorHandlerService.registerErrorHandler(DatabaseErrorHandlerService);
	}
}


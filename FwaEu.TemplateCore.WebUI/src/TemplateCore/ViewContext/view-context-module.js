import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';
import ViewContextService from './Services/view-context-service';

export class ContextViewModule extends AbstractModule {

	async onInitAsync() {
		await ViewContextService.configure();
	}
}
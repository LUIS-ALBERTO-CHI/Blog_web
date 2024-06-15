import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';
import LoadingPanelService from './Services/loading-panel-service';
import { createVueWait } from 'vue-wait';

export class LoadingPanelModule extends AbstractModule {
	async onInitAsync(vueApp) {
		const vueWait = createVueWait({ registerComponent: true });
		vueApp.use(vueWait);
		LoadingPanelService.configure(vueWait.instance);
	}
}



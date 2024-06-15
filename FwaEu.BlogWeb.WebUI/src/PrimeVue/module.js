import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';
import PrimeVue from 'primevue/config';
import 'primevue/resources/themes/bootstrap4-light-blue/theme.css';
import 'primevue/resources/primevue.min.css';
import 'primeflex/primeflex.scss';
import 'primeicons/primeicons.css';
import 'primeflex/primeflex.css';
import Tooltip from 'primevue/tooltip';

const componentMapping = [];

export function addComponentMapping(source, primeVueTarget) {
	componentMapping.push({ source: source, primeVueTarget: primeVueTarget });
}

export function resolveComponentName(componentName) {
	return componentMapping.find(e => e.source == componentName)?.primeVueTarget ?? componentName ?? "InputText";
}
export class UILibraryModule extends AbstractModule {
	async onInitAsync(vueApp) {
		
		vueApp.use(PrimeVue, {
			zIndex: {
				modal: 1700,        //dialog, sidebar
				overlay: 1700,      //dropdown, overlaypanel
				menu: 1700,         //overlay menus
				tooltip: 1700,      //tooltip
			}
		});
		vueApp.directive('tooltip', Tooltip);
	}
}

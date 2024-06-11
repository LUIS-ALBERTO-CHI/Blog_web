import { createApp, defineComponent, h } from 'vue';
import { RouterLink } from 'vue-router';
import Router from './vue-router-service';

export const createRouterLink = (props, defaultSlot) => {
	const componentDefinition = defineComponent({
		render() {
			return h(RouterLink, { ...this.$props }, () => defaultSlot);
		}
	});
	const vueApp = createApp(componentDefinition, props);
	vueApp.use(Router);
	const vueAppContainer = document.createElement('div');
	const componentInstance = vueApp.mount(vueAppContainer);
	componentInstance.unmount = function () {
		vueApp.unmount.bind(vueApp)();
		vueAppContainer.remove();
	}
	return componentInstance;
};

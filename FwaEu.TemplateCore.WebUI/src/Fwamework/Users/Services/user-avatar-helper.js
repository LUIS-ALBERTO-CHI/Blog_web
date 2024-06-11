import { createApp, defineComponent } from 'vue';
import UserAvatar from '@/Fwamework/Users/Components/UserAvatarComponent.vue';

export const createUserAvatar = (props) => {
	const componentDefinition = defineComponent({
		extends: defineComponent({ ...UserAvatar })
	});
	const vueApp = createApp(componentDefinition, props);

	const vueAppContainer = document.createElement('div');
	const componentInstance = vueApp.mount(vueAppContainer);
	componentInstance.unmount = function () {
		vueApp.unmount.bind(vueApp)();
		vueAppContainer.remove();
	}
	return componentInstance;
};

import MenuService from '@/Fwamework/Menu/Services/menu-service';
import AsyncEventEmitter from '@/Fwamework/Core/Services/event-emitter-class';

export const NavigationMenuType = 'sideNavigation';
class NavigationMenuService extends MenuService {
	mountedEvent = new AsyncEventEmitter();
	unmountedEvent = new AsyncEventEmitter();

	constructor() {
		super(NavigationMenuType);
	}

	onMounted(listener) {
		return this.mountedEvent.addListener(listener);
	}

	onUnmounted(listener) {
		return this.unmountedEvent.addListener(listener);
	}
}
export default new NavigationMenuService();
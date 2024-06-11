import InMemoryStore from "@/Fwamework/Storage/Services/in-memory-store";

/**
 * In memory store that will persist the data only during current route
 * */
class RoutingCacheStore extends InMemoryStore {
	/**
	 * @param {import('vue-router').default} router
	 */
	configure(router) {
		const $this = this;

		router.isReady().then(() => {
			router.beforeEach((to, from, next) => {
				$this.storedItems.clear();
				next();
			});
		});
	}
}

export default new RoutingCacheStore();
import AsyncEventEmitter from "@/Fwamework/Core/Services/event-emitter-class";

const nodeResolved = new AsyncEventEmitter();
const nodeCreated = new AsyncEventEmitter();
const routeProcessed = new AsyncEventEmitter();
const mountedEvent = new AsyncEventEmitter();
const unmountedEvent = new AsyncEventEmitter();
const nodeClicked = new AsyncEventEmitter();

/** @typedef {{resolvingRoute: import("vue-router").Route, context: import('./resolve-context').default, node: import('./resolve-context').BreadcrumbNode }} NodeEventArgs */

export default {

	mountedEvent,
	unmountedEvent,
	nodeClicked,

	onNodeClicked(listener) {
		return nodeClicked.addListener(listener);
	},
	/** @param {(e: NodeEventArgs) => Promise} listener */
	onNodeCreated(listener) {
		return nodeCreated.addListener(listener);
	},

	/** @param {(e: NodeEventArgs) => Promise} listener */
	onNodeResolved(listener) {
		return nodeResolved.addListener(listener);
	},
	/** @param {(e: NodeEventArgs) => Promise} listener */
	onRouteProcessed(listener) {
		return routeProcessed.addListener(listener);

	},
	onMounted(listener) {
		return mountedEvent.addListener(listener);
	},
	onUnmounted(listener) {
		return unmountedEvent.addListener(listener);
	},
	/**@param {import("vue-router").Route} route
	 * @param {import('./resolve-context').default} context
	 */
	async processRouteAsync(route, context) {
		const breadcrumbNodes = await processNodeAsync(route, context);
		await routeProcessed.emitAsync({ breadcrumbNodes });
		return breadcrumbNodes;

	}
};

/**
* Gets the information about the current route and adds it in the breadcrumbs("resolvedNodes")
* @param {import("vue-router").Route} route
* @param {import('./resolve-context').default} context
 * @returns {Promise<Array<import('./resolve-context').BreadcrumbNode>>}
*/
async function processNodeAsync(route, context) {

	if (route.meta && route.meta.breadcrumb) {
		let currentNode = await createNodeAsync(route, context);

		const breadcrumbInfo = route.meta.breadcrumb;
		const currentComponent = context.router.currentRoute.value?.matched[0].instances.default;
		context.currentComponent = currentComponent;
		//Call onNodeResolve if current node is same with the node being called. 
		if (route.name === context.router.currentRoute.value.name) {

			let componentOnNodeResolveAsync = currentComponent?.onNodeResolve;

			if (componentOnNodeResolveAsync) {
				componentOnNodeResolveAsync = componentOnNodeResolveAsync.bind(currentComponent);
				// Gets the dynamic information (titleKey, parentName) if present, about breadcrumb from the current route
				currentNode = await componentOnNodeResolveAsync(currentNode, context);
			}
		}

		if (breadcrumbInfo.onNodeResolve) {
			// Gets the dynamic information (titleKey, parentName) if present, about breadcrumb from the current route
			currentNode = await breadcrumbInfo.onNodeResolve(currentNode, context);
		}

		if (currentNode) {
			await addResolvedNodeAsync(route, context, currentNode);
			await resolveParentNodeAsync(currentNode, context);
		}
	}
	else {
		await addResolvedNodeAsync(route, context, { text: 'Missing "meta.breadcrumb"', to: route.path });
	}
	return context.resolvedNodes;
}

// Gets the parent of the current node if any, resolving its route and finally adding in the breadcrumbs("resolvesNodes")
async function resolveParentNodeAsync(currentNode, context) {
	let parentName = currentNode.parentNode;
	if (parentName) {
		let parentRoute = getRouteByName(parentName, context);
		if (parentRoute) {
			await processNodeAsync(parentRoute, context);
		}
		else {
			await addResolvedNodeAsync(null, context, { text: `Not found node "${parentName}"`, to: '' });
		}
	}
}

async function addResolvedNodeAsync(resolvingRoute, context, node) {
	await nodeResolved.emitAsync({ resolvingRoute, context, node });
	context.resolvedNodes.push(node);
}

//Searches the static information(titleKey, parentName) if present, about breadcrumb from the current route
async function createNodeAsync(route, context) {
	const breadcrumbInfo = route.meta.breadcrumb;
	const nodeText = breadcrumbInfo.title ?? (breadcrumbInfo.titleKey ? context.i18n.t(breadcrumbInfo.titleKey) : 'Missing "meta.breadcrumb.titleKey"');
	const node = { text: nodeText, to: route.path, parentNode: breadcrumbInfo.parentName };
	await nodeCreated.emitAsync({ context, node });

	return node;
}

//Gets the route's information by its name.
function getRouteByName(routeName, context) {
	let node = context.router.resolve({ name: routeName }, context.router.currentRoute.value);
	return node;
}

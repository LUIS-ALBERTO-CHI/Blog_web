const context = import.meta.glob('/**/routes.js', { eager: true });
const Setup = () => import('@/Fwamework/Setup/Components/SetupAppComponent.vue');

let globalRoutes = [
    {
        name: 'default',
        path: "/",
        component: Setup,
        meta: {
            allowAnonymous: true,
            breadcrumb: {
                titleKey: 'homeTitle'
            }
        }
    },
    {
		path: '/:pathMatch(.*)',
        redirect: "/"
    }
];

Object.keys(context).forEach(function (path) {
	let exportedModule = context[path];
	let exportedRoutes = exportedModule.default;
    if (exportedRoutes) {
        globalRoutes = globalRoutes.concat(exportedRoutes);
    }
});

export default globalRoutes;
const GenericAdminPageComponent = () => import('@/Modules/GenericAdmin/Components/GenericAdminPageComponent.vue');

export default [
	{
		path: '/GenericAdmin/:configurationKey',
		name: 'GenericAdmin',
		component: GenericAdminPageComponent,
		meta: {
			zoneName: 'admin',
			breadcrumb: {
				titleKey: 'genericAdminTitle',
				parentName: 'Administration'
			}
		}
	}
]
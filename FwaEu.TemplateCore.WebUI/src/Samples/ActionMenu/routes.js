const SampleActionMenu = () => import('@/Samples/ActionMenu/Components/SampleActionMenuPageComponent.vue');

export default [

	{
		path: '/SampleActionMenu',
		name: 'SampleActionMenu',
		component: SampleActionMenu,
		meta: {
			breadcrumb: {
				title: 'Action menu sample',
				parentName: 'default'
			}
		}
	}
	
]
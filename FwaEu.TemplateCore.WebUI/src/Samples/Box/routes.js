const SampleBox = () => import('@/Samples/Box/Components/SampleBoxPageComponent.vue');

export default [

	{
		path: '/SampleBox',
		name: 'SampleBox',
		component: SampleBox,
		meta: {
			breadcrumb: {
				title: 'Box sample',
				parentName: 'default'
			}
		}
	}
	
]
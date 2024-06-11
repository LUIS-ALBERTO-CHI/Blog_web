const LayoutSample = () => import('@/Samples/Layout/Components/LayoutSamplePageComponent.vue');

export default [

	{
		path: '/LayoutSample',
		name: 'LayoutSample',
		component: LayoutSample,
		meta: {
			breadcrumb: {
				title: 'Layout sample',
				parentName: 'default'
			}
		}
	}
]
const SampleUserToolTipPage = () => import('@/Samples/UserToolTip/Components/SampleUserToolTipPageComponent.vue');

export default [

	{
		path: '/SampleUserToolTip',
		name: 'SampleUserToolTip',
		component: SampleUserToolTipPage,
		meta: {
			breadcrumb: {
				title: 'User tool tip sample',
				parentName: 'default'
			}
		}
	}
	
]
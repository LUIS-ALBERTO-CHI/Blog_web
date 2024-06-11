const ErrorSample = () => import('@/Samples/Error/Components/DisplayErrorsPageComponent.vue');

export default [

	{
		path: '/ErrorSample',
		name: 'ErrorSample',
		component: ErrorSample,
		meta: {
			breadcrumb: {
				title: 'Error page sample',
				parentName: 'default'
			}
		}
	}
	
]
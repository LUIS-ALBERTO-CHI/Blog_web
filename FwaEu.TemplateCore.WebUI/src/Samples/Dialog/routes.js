const SampleDialog = () => import('@/Samples/Dialog/Components/SampleDialogPageComponent.vue');

export default [

	{
		path: '/SampleDialog',
		name: 'SampleDialog',
		component: SampleDialog,
		meta: {
			breadcrumb: {
				title: 'Dialog sample',
				parentName: 'default'
			}
		}
	}
	
]
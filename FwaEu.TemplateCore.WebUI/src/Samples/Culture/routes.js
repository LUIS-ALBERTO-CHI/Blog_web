const LocalizationSample = () => import('@/Samples/Culture/Components/LocalizationSamplePageComponent.vue');
export default [
	{
		path: '/LocalizationSample',
		name: 'LocalizationSample',
		component: LocalizationSample,
		meta: {
			breadcrumb: {
				title: 'Localization',
				parentName: 'default'
			}
		}
	}]
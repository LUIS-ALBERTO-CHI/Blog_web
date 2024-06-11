const SampleLoadingPanel = () => import('@/Samples/LoadingPanel/Components/SampleLoadingPanelPageComponent.vue');

export default [{
	path: '/SampleLoadingPanel',
	name: 'SampleLoadingPanel',
	component: SampleLoadingPanel,
	meta: {
		breadcrumb: {
			title: 'Loading panel',
			parentName: 'default'
		}
	}
}];
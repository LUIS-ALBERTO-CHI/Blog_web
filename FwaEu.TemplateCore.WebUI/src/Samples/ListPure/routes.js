const SampleListPure = () =>
    import ('@/Samples/ListPure/Components/ListPurePageComponent.vue');

export default [{
    path: '/SampleListPure',
    name: 'SampleListPure',
    component: SampleListPure,
    meta: {
        zoneName: 'admin',
        breadcrumb: {
			title: 'List sample',
            parentName: 'default'
        }
    }
}];
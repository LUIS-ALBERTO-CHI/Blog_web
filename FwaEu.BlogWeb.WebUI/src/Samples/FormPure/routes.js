const SampleFormPure = () =>
    import ('@/Samples/FormPure/Components/FormPurePageComponent.vue');

export default [{
    path: '/SampleFormPure',
    name: 'SampleFormPure',
    component: SampleFormPure,
    meta: {
        zoneName: 'admin',
        breadcrumb: {
			title: 'Form sample',
            parentName: 'default'
        }
    }
}];
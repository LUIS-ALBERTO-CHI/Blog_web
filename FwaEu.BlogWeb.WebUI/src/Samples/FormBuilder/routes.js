const SampleFormBuilder = () =>
    import ('@/Samples/FormBuilder/Components/FormBuildingSample.vue');

export default [{
    path: '/SampleFormBuilder',
    name: 'SampleFormBuilder',
	component: SampleFormBuilder,
    meta: {
        zoneName: 'admin',
        breadcrumb: {
			title: 'Form builder sample',
            parentName: 'default'
        }
    }
}];
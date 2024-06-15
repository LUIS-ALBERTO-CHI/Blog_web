const SampleSummaryPure = () =>
    import ('@/Samples/SummaryPure/Components/SummaryPurePageComponent.vue');

export default [{
    path: '/SampleSummaryPure',
    name: 'SampleSummaryPure',
    component: SampleSummaryPure,
    meta: {
        zoneName: 'admin',
        breadcrumb: {
            title: 'Summary sample',
            parentName: 'default'
        }
    }
}];
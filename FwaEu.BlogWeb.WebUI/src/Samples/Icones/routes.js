const SampleIcone = () =>
    import ('@/Samples/Icones/Components/SampleIconeComponent.vue');

export default [

    {
        path: '/SampleIcone',
        name: 'SampleIcone',
        component: SampleIcone,
        meta: {
            breadcrumb: {
                title: 'Icone sample',
                parentName: 'default'
            }
        }
    }

]
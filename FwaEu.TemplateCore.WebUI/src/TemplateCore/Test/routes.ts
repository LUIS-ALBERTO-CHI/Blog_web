import TestPage from "@/TemplateCore/Test/Components/TestComponent.vue";

export default [
	{
		path: '/Test',
		name: 'Test',
		component: TestPage,
		meta: {
			breadcrumb: {
				titleKey: 'testpage',
				parentName: 'default'
			},
		}
	},
]
const Page1 = () => import('@/Samples/Breadcrumbs/Components/Page1PageComponent.vue');
const Page2 = () => import('@/Samples/Breadcrumbs/Components/Page2PageComponent.vue');
const Page3 = () => import('@/Samples/Breadcrumbs/Components/Page3PageComponent.vue');
const Page4 = () => import('@/Samples/Breadcrumbs/Components/Page4PageComponent.vue');

export default [

	{
		path: '/Page1',
		name: 'Page1',
		component: Page1,
		meta: {
			breadcrumb: {
				//TODO: En attente du dev lié au ticket https://dev.azure.com/fwaeu/TemplateCore/_workitems/edit/3522
				title: 'Première page', 
				parentName: 'default'
			},
			zoneName: 'Zone 1'
		}
	},
	{
		path: '/Page2',
		name: 'Page2',
		component: Page2,
		meta: {
			breadcrumb: {
				//TODO: Waiting for dev related to ticket https://dev.azure.com/fwaeu/TemplateCore/_workitems/edit/3522
				title: 'Deuxième page',
				parentName: 'Page1'
			},
			zoneName: 'Zone 2'
		}
	},

	{
		path: '/Page3',
		name: 'Page3',
		component: Page3,
		meta: {
			breadcrumb: {
				//TODO: Waiting for dev related to ticket https://dev.azure.com/fwaeu/TemplateCore/_workitems/edit/3522
				title: 'Troisième page',
				parentName: 'Page2',
			},
			zoneName: 'Zone 3'
		}
	},
	{
		path: '/Page4',
		name: 'Page4',
		component: Page4,
		meta: {
			breadcrumb: {
				// This function can accept two parameters: onNodeResolve(node, context)
				onNodeResolve(node) {
					const name = "Dernière page";
					node.parentNode = 'Page1';
					node.to = '';
					node.text = name;
					return node;
				}
			},
			zoneName: 'Zone 4'
			
		}
	}
];
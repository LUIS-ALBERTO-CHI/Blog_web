const ReportsPageComponent = () => import('@UILibrary/Modules/ReportAdmin/Components/ReportsPageComponent.vue');
const ReportPageComponent = () => import('@UILibrary/Modules/ReportAdmin/Components/ReportPageComponent.vue');
const ReportViewsPageComponent = () => import('@UILibrary/Modules/ReportAdmin/Components/ReportViewsPageComponent.vue');

import { CanAdministrateReports } from '@/Modules/ReportAdmin/report-admin-permissions';

export default [
	{
		path: '/ReportAdmin',
		name: 'ReportAdmin',
		component: ReportsPageComponent,
		meta: {
			zoneName: 'admin',
			breadcrumb: {
				titleKey: 'reportAdminPage',
				parentName: 'Administration'
			},
			requiredPermissions: [CanAdministrateReports]
		}
	},
	{
		path: '/ReportAdmin/Create',
		name: 'NewReportDetails',
		component: ReportPageComponent,
		meta: {
			zoneName: 'admin',
			breadcrumb: {
				titleKey: "newReport",
				parentName: "ReportAdmin"
			},
			requiredPermissions: [CanAdministrateReports]
		}
	},
	{
		path: '/ReportAdmin/CreateFrom/:invariantId',
		name: 'NewReportDetailsFromExisting',
		component: ReportPageComponent,
		meta: {
			zoneName: 'admin',
			breadcrumb: {
				titleKey: "newReport",
				parentName: "ReportAdmin"
			},
			requiredPermissions: [CanAdministrateReports]
		},
		props: (route) => ({ cloneInvariantId: route.params.invariantId })
	},
	{
		path: '/ReportAdmin/:invariantId',
		name: 'EditReportDetails',
		component: ReportPageComponent,
		meta: {
			zoneName: 'admin',
			breadcrumb: {
				parentName: "ReportAdmin",
				onNodeResolve(node, context) {
					if (typeof context.currentComponent.invariantId !== "string") {
						throw new Error("Children pages of report admin must implement a invariantId string property");
					}
					node.text = context.currentComponent.invariantId;

					return node;
				}
			},
			requiredPermissions: [CanAdministrateReports]
		},
		props: (route) => ({ invariantId: route.params.invariantId })
	},
	{
		path: '/ReportAdmin/ManageViews/:invariantId',
		name: 'ManageViews',
		component: ReportViewsPageComponent,
		meta: {
			zoneName: 'admin',
			breadcrumb: {
				parentName: "ReportAdmin",
				onNodeResolve(node, context) {
					if (typeof context.currentComponent.invariantId !== "string") {
						throw new Error("Children pages of report admin must implement a invariantId string property");
					}
					node.text = context.currentComponent.invariantId;

					return node;
				}
			}
		},
		props: (route) => ({ invariantId: route.params.invariantId })
	},
]

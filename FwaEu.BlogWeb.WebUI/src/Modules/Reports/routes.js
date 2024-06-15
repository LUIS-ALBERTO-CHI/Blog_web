const ReportFilterPageComponent = () => import('@UILibrary/Modules/Reports/Components/ReportFilterPageComponent.vue');
const ReportsPageComponent = () => import('@/Modules/Reports/Components/ReportsPageComponent.vue');
const ReportPageComponent = () => import('@/Modules/Reports/Components/ReportPageComponent.vue');
import { CanViewReports } from "@/Modules/Reports/reports-permissions";

export default [
	{
		path: '/Reports',
		name: 'Reports',
		component: ReportsPageComponent,
		meta: {
			zoneName: 'admin',
			breadcrumb: {
				titleKey: 'reports',
				parentName: 'default'
			},
			requiredPermissions: [CanViewReports]
		}
	},
	{
		path: '/Reports/:invariantId/Filters',
		name: 'ReportFilter',
		component: ReportFilterPageComponent,
		meta: {
			zoneName: 'admin',
			breadcrumb: {
				parentName: 'Reports'
			},
			requiredPermissions: [CanViewReports]
		},
		props: (route) => ({ invariantId: route.params.invariantId, filterValues: route.query.filters ? JSON.parse(route.query.filters) : null })
	},
	{
		path: '/Reports/:invariantId',
		name: 'Report',
		component: ReportPageComponent,
		meta: {
			zoneName: 'admin',
			breadcrumb: {
				parentName: 'Reports'
			},
			requiredPermissions: [CanViewReports]
		},
		props: (route) => ({ invariantId: route.params.invariantId, filterValues: route.query.filters ? JSON.parse(route.query.filters) : null })
	}
]
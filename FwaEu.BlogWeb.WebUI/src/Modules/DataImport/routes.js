import { CanImport } from '@/Modules/DataImport/data-import-permissions';

const ImportComponent = () => import('@UILibrary/Modules/DataImport/Components/ImportPageComponent.vue');

export default [
	{
		path: '/Import',
		name: 'Import',
		component: ImportComponent,
		meta: {
			zoneName: 'admin',
			breadcrumb: {
				titleKey: 'dataImportMenuItemText',
				parentName: 'Administration'
			},
			requiredPermissions: [CanImport]
		}
	}
	
]
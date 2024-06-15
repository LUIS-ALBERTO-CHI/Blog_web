const UserSettingsPageComponent = () => import('@UILibrary/Fwamework/UserSettings/Components/UserSettingsPageComponent.vue');

export default [
	{
		path: '/UserSettings',
		name: 'UserSettings',
		component: UserSettingsPageComponent,
		meta: {
			allowAnonymous: true,
			breadcrumb: {
				titleKey: 'userSettingsMenuItemText',
				parentName: 'default'
			}
		}
	}
]
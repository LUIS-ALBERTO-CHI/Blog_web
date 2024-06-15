import { CanAdministrateUsers } from '@/Fwamework/Users/users-permissions';
const UsersPageComponent = () => import('@UILibrary/Fwamework/Users/Components/UsersPageComponent.vue');
const UserDetailsPageComponent = () => import('@UILibrary/Fwamework/Users/Components/UserDetailsPageComponent.vue');

export default [
	{
		path: '/Users',
		name: 'Users',
		component: UsersPageComponent,
		meta: {
			zoneName: 'admin',
			breadcrumb: {
				titleKey: 'usersTitle',
				parentName: 'Administration'
			},
			requiredPermissions: [CanAdministrateUsers]
		}
	},
	{
		path: '/Users/Create',
		name: 'NewUserDetails',
		component: UserDetailsPageComponent,
		meta: {
			zoneName: 'admin',
			breadcrumb: {
				titleKey: 'newUser',
				parentName: 'Users'
			},
			requiredPermissions: [CanAdministrateUsers]
		}
	},
	{
		path: '/Users/:id',
		name: 'EditUserDetails',
		component: UserDetailsPageComponent,
		meta: {
			zoneName: 'admin',
			breadcrumb: {
				parentName: 'Users'
			},
			requiredPermissions: [CanAdministrateUsers]
		}
	}
]
const UserAvatarPageComponent = () => import('@/Samples/UserAvatar/Components/UserAvatarPageComponent.vue');

export default [
	{
		path: '/Sample/UserAvatar',
		name: 'SampleUserAvatar',
		component: UserAvatarPageComponent,
		meta: {
			breadcrumb: {
				title: 'Sample User Avatar',
				parentName: 'default'
			}
		}
	}
]
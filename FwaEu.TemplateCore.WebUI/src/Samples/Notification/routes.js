const Notification = () => import('@/Samples/Notification/Components/NotificationPageComponent.vue');
const UserNotification = () => import('@/Samples/Notification/Components/UserNotificationPageComponent.vue');

export default [

	{
		path: '/SampleNotification',
		name: 'SampleNotification',
		component: Notification,
		meta: {
			breadcrumb: {
				title: 'Notification',
				parentName: 'default'
			}
		}
	},
	{
		path: '/SampleUserNotification',
		name: 'SampleUserNotification',
		component: UserNotification,
		meta: {
			breadcrumb: {
				title: 'User notification',
				parentName: 'default'
			}
		}
	}
	
]
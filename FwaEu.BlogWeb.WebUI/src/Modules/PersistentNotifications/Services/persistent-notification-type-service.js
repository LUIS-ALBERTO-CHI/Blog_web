const persistentNotificationFilesContext = import.meta.glob('/**/*-persistent-notification.js', { eager: true});

export default {
    async getAllAsync() {
		return Object.keys(persistentNotificationFilesContext).map(path => persistentNotificationFilesContext[path].default)
    }
}
const context = import.meta.glob('/**/*-setup-task.js', { import: 'default', eager: true });

export default {
	getAll() {
		return Object.values(context);
	}
};
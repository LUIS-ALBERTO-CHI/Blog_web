const context = import.meta.glob('/**/*-perimeter-type.js', { import: 'default', eager: true });

export default {
	getAllPerimeterTypeProviders() {
		return Object.values(context);
	}
};
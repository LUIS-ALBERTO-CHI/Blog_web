export default class CustomColumnCustomizer {
	CustomColumnCustomizer() {
		throw 'Cannot directly implement CustomColumnCustomizer';
	}

	async customizeAsync(columns, properties) {
		return columns;
	}
}

export default {
	addTextNormalization() {
		String.prototype.getNormalizedText = function () {
			return this.normalize("NFD").replace(/[\u0300-\u036f]/g, "");
		}
	}
};

export default {
	convertBase64ToBlob(base64String, fileType) {
		let byteString = atob(base64String);
		let buffer = new ArrayBuffer(byteString.length);
		let ia = new Uint8Array(buffer);

		for (let i = 0; i < byteString.length; i++) {
			ia[i] = byteString.charCodeAt(i);
		}
		return new Blob([buffer], { type: fileType });
	},

	/**
	 * @param {File} file
	 * @returns {Promise<String>}
	 */
	async convertFileToBase64Async(file) {
		return new Promise((resolve) => {
			let reader = new FileReader();
			reader.onload = function () {
				let bytes = new Uint8Array(reader.result);
				let length = bytes.byteLength;
				let binary = '';
				for (let i = 0; i < length; i++) {
					binary += String.fromCharCode(bytes[i]);
				}
				let base64String = btoa(binary);
				resolve(base64String);
			};
			reader.readAsArrayBuffer(file);
		});
	},

	getFileName(fullFilePath) {
		let filePathParts = fullFilePath.split('/');
		let fileName = filePathParts[filePathParts.length - 1];
		return fileName;
	}
}
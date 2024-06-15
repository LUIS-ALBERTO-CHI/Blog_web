export default {

	createFileManagerFiles(files) {
		let allFileItems = [];
		let lastDirectory = null;
		files.forEach(file => {
			let filePathParts = file.filePath.split("/");
			if (filePathParts.length > 0) {
				if (filePathParts[0] === '') {
					filePathParts.shift();
				}
				for (let filePathPart of filePathParts) {
					let isLast = filePathParts.indexOf(filePathPart) === filePathParts.length - 1;
					let itemsListToUse = lastDirectory?.items ?? allFileItems;
					let existingPart = itemsListToUse.find(it => it.name === filePathPart);
					if (existingPart) {
						lastDirectory = existingPart;
					} else {
						let newFileItem = { name: filePathPart, isDirectory: !isLast };
						if (newFileItem.isDirectory) {
							newFileItem['items'] = [];

							lastDirectory = newFileItem;

						} else {
							newFileItem['id'] = file.id;
							newFileItem['size'] = file.fileLengthInBytes;
						}
						itemsListToUse.push(newFileItem);
					}
				}
			}
			lastDirectory = null;
		});
		return allFileItems;
	}
}

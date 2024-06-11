import { globSync } from 'glob';
import { resolve, extname } from 'path';

export default function (uiLibraryPath, fallbackUiLibrary) {

	let fallBackUiLibComponents = [];
	let uiLibComponents = [];
	let fallbackComponents = [];
	let targetLibrary = {};

	if (uiLibraryPath != fallbackUiLibrary) {

		fallBackUiLibComponents = globSync('**/*.*', { cwd: fallbackUiLibrary });

		uiLibComponents = globSync('**/*.*', { cwd: uiLibraryPath });

		fallbackComponents = fallBackUiLibComponents.filter(u => !(uiLibComponents.includes(u)));

		fallbackComponents.forEach(function (comPath) {
			targetLibrary["@UILibrary/" + comPath.replaceAll('\\', '/') .replace(extname(comPath), "")] = resolve(fallbackUiLibrary, comPath);
			targetLibrary["@UILibrary/" + comPath.replaceAll('\\', '/')] = resolve(fallbackUiLibrary, comPath);
		});
	}

	return targetLibrary;

}

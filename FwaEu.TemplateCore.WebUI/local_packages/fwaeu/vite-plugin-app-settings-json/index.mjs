import fs from 'fs';
import path from 'path';
import { merge } from 'lodash-es';
import json5 from 'json5';
import { join } from 'path';

/** 
 *  @param {{rootPath: string, externalize: boolean }} options 
 *  If options.externalize is true the appSettings will be saved into an external appSettings.js and served via an injected <script> tag within the HTML page(s)
 *  If options.externalize is false the appSettings will be bundled with the application */
export default function (options) {
	const rootPath = options?.rootPath;
	if (!rootPath)
		throw new Error('options.rootPath is required');
	const externalize = options.externalize ?? false;
	const virtualModuleId = '@fwa/virtual-vite-plugin-app-settings'
	const resolvedVirtualModuleId = '\0' + virtualModuleId
	const outputFileName = `appSettings.${new Date().getTime()}.js`;

	let fullAppSettings = {};
	let finalConfiguration;
	const loadEnv = function (config, mode) {

		const load = appSettingsPathWithoutExtension => {
			try {
				const currentFileFullPath = fs.realpathSync.native(path.resolve(rootPath, `${appSettingsPathWithoutExtension}.json`));
				if (config.loadedAppSettingFiles.includes(currentFileFullPath))
					return;

				const appSettingsText = fs.readFileSync(currentFileFullPath, { encoding: "utf8" });

				//NOTE: We use JSON5 for parsing in order to allow comments within the json files
				//More information: https://json5.org/
				const appSettings = json5.parse(appSettingsText);
				fullAppSettings = merge(fullAppSettings, appSettings);

				config.loadedAppSettingFiles.push(currentFileFullPath.replaceAll('\\', '/'));

			} catch (err) {
				// only ignore error if file is not found
				if (err.toString().indexOf('ENOENT') < 0) {
					throw err;
				}
			}
		}

		for (const environmentNamePart of String(mode).split(".")) {

			let basePathWithoutExtension = `appsettings${(environmentNamePart !== 'undefined') ? `.${environmentNamePart}` : ``}`;
			let localPathWithoutExtension = `${basePathWithoutExtension}.local`;

			load(basePathWithoutExtension);
			load(localPathWithoutExtension);

		}

		load(`appsettings.${mode}`);
		load(`appsettings.${mode}.local`);

	};


	return {
		name: 'app-settings-json',
		config(config, { mode }) {

			config.loadedAppSettingFiles = [];
			console.log('Loading app settings for mode: ' + mode);

			// load base appsettings
			loadEnv(config);

			// load mode appsettings
			if (mode) {
				loadEnv(config, mode);
			}
		
			
			return { ...(fullAppSettings.vitejs || {}), appSettings: fullAppSettings, loadedAppSettingFiles: config.loadedAppSettingFiles };
		},

		configResolved(resolvedConfig) {
			resolvedConfig.configFileDependencies.push(...resolvedConfig.loadedAppSettingFiles);
			finalConfiguration = resolvedConfig
			if (externalize) {
				fs.writeFileSync(join(rootPath, 'public', outputFileName), `window.appSettings = ${json5.stringify(finalConfiguration.appSettings)}`);
			}
		},
		transformIndexHtml(html, { filename }) {

			let transformedHtml = html.replace(/{{([\w\s\n.]+?)}}/g, function (m, m1) {
				let tempObj = finalConfiguration;
				const foudValue = m1 ? m1.split(".").map(property => {
					tempObj = tempObj[property];
					return tempObj;
				}).slice(-1) : null;
				const resolvedValue = foudValue && foudValue.length ? foudValue[0] : m;

				return resolvedValue;
			});
			if (externalize) {
				//NOTE: Use the first script tag in order to inject the appSettings.js script tag
				transformedHtml = transformedHtml.replace('<script', `<script type="module" src="/${outputFileName}"></script>
<script`);
			}
			return transformedHtml;
		},
		resolveId(id) {
			if (id === virtualModuleId) {
				return resolvedVirtualModuleId
			}
		},
		load(id) {
			if (id === resolvedVirtualModuleId) {
				const moduleContent = externalize ? '{ ...window.appSettings }' : json5.stringify(finalConfiguration.appSettings);
				return `export default  ${moduleContent}`;
			}
		}
	};
};
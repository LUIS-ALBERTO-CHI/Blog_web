import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';
import VueI18nPlugin from '@intlify/unplugin-vue-i18n/vite';
import appSettingsJson from '@fwaeu/vite-plugin-app-settings-json';
import uiLibrarySwitch from '@fwaeu/vite-plugin-uilibrary-switch';
import { resolve } from "path";
import { VitePWA } from 'vite-plugin-pwa';
import checker from 'vite-plugin-checker'
import { dirname } from 'path';

const rootPath = dirname('');

const srcPath = resolve(rootPath, "./src");
const uiLibraryPath = resolve(rootPath, "./src/PrimeVue"); // Change if another UILibrary is used
const fallbackUiLibrary = resolve(rootPath, "./src/DevExtreme"); // Fallback or default option

export default defineConfig(config => ({
	plugins: [
		appSettingsJson({
			rootPath,
			externalize: false//NOTE: Set to true if you want to externalize the settings into an appSettings.js file
		}),
		vue(),
		VueI18nPlugin({
			fullInstall: false,
			runtimeOnly: false,
			include: resolve(srcPath, '/**/HACK-UNEXISTING_PATH.json')
		}),
		checker({
			// e.g. use TypeScript check
			typescript: true,
			vueTsc: true
		}),
		VitePWA({
			registerType: 'autoUpdate',
			injectRegister: 'inline',
			devOptions: {
				enabled: true
			},
			includeAssets: ['favicon.ico', 'apple-touch-icon.png', 'masked-icon.svg'],
			manifest: {
				"lang": "fr-fr",
				"android_package_name": "fwaeu.templateCore",
				"name": "BlogWeb",
				"short_name": "BlogWeb",
				"description": "BlogWeb",
				"start_url": "https://wild.fwa.eu/BlogWeb/",
				"background_color": "#0078D4",
				"theme_color": "#0078D4",
				"orientation": "any",
				"display": "standalone",
				"icons": [
					{
						"src": "/icon512.png",
						"sizes": "512x512"
					}
				]
			}

		})],
	resolve: {
		alias: {
			"@": srcPath,
			...uiLibrarySwitch(uiLibraryPath, fallbackUiLibrary),
			"@UILibrary": uiLibraryPath,
			'vue-i18n': 'vue-i18n/dist/vue-i18n.esm-bundler.js',
			"devextreme/ui": 'devextreme/esm/ui' // Hack: Bug devextreme fails in production due to the different behavior of vite (than that of webpack) to import the compiled modules
			//https://supportcenter.devexpress.com/ticket/details/t1054272/vue3-react-vite-rollup-devextreme-fails-in-production-because-some-modules-do-not-pass

		}
	},
	build: {
		chunkSizeWarningLimit: 2000,
		rollupOptions: {
			input: {
				main: resolve(rootPath, 'index.html'),
				nested: resolve(rootPath, 'setup.html')
			}
		}
	},
	test: {
		dir: resolve(srcPath, 'test'),
		setupFiles: [resolve(srcPath, 'test/setup/setup.js')]
	}
}))
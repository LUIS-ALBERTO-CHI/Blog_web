import ModuleRegistry from '@/Fwamework/Core/Services/module-registry';
import { I18n } from '@/Fwamework/Culture/Services/localization-service';
import AsyncEventEmitter from '@/Fwamework/Core/Services/event-emitter-class';
import cloneDeep from 'lodash/cloneDeep';

export default class MenuService {
	_menuItems = null;
	_menuItemsUpdatedEventEmitter = new AsyncEventEmitter();

	constructor(menuType) {
		if (!menuType) {
			throw new Error("You must provide a menuType");
		}
		this.menuType = menuType;		
	}

	/** @param {(Array) => Promise} listener */
	onMenuItemsUpdated(listener) {
		return this._menuItemsUpdatedEventEmitter.addListener(listener);
	}

	async anyMenuItemAvailableAsync() {
		const menuItems = await this.getMenuItemsAsync();

		return menuItems.length > 0;
	}

	/** @returns {Promise<Array>} */
	async getMenuItemsAsync() {

		if (!this._menuItems) {
			await this.reloadMenuItemsAsync();
		}
		return cloneDeep(this._menuItems);//NOTE: return a copy in order to prevent accidental updates of menu items
	}

	async setMenuItemsAsync(menuItems) {
		this._menuItems = menuItems;
		await this._menuItemsUpdatedEventEmitter.emitAsync(menuItems);
	}

	async reloadMenuItemsAsync() {
		let menuItems = [];

		let allModules = ModuleRegistry.getAll();
		for (const module of allModules) {
			let currentModuleItems = await module.getMenuItemsAsync(this.menuType);
			menuItems = menuItems.concat(currentModuleItems);
		}

		this.localizeMenuItems(menuItems);

		menuItems.sort((a, b) => (a.visibleIndex ?? 1000) - (b.visibleIndex ?? 1000));
		this._menuItems = menuItems;
		await this._menuItemsUpdatedEventEmitter.emitAsync(cloneDeep(menuItems));//NOTE: return a copy in order to prevent accidental updates of menu items
	}

	localizeMenuItems(menuItems) {
		menuItems.forEach(menuItem => {
			if (menuItem.textKey) {
				menuItem.text = I18n.t(menuItem.textKey);
			}
			if (menuItem.items) {
				for (let submenuItem of menuItem.items) {
					if (submenuItem.textKey) {
						submenuItem.text = I18n.t(submenuItem.textKey);
					}
				}
			}
		});
		menuItems.filter(mi => mi.descriptionKey).forEach(mi => {
			mi.descriptionText = I18n.t(mi.descriptionKey);
		});
	}
}

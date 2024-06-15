
export const selectedMenuItemClass = "selected-menu-item";
export const parentSelectedMenuItemClass = "parent-selected-menu-item";

export default {
	removeSelectedClass(menuItems) {
		for (let menuItem of menuItems) {
			delete menuItem.selectedClass;
			menuItem.expanded = false;

			if (menuItem.items)
				this.removeSelectedClass(menuItem.items)
		}
	},
	searchMenuItemByRoutePath(menuItem, router, path) {
		if (menuItem.path && router.resolve(menuItem.path).path === path) {
			return menuItem;
		}
		else if (menuItem.items) {
			for (var childItem of menuItem.items) {
				childItem.parentMenuItem = menuItem;
				const foundMenuItem = this.searchMenuItemByRoutePath(childItem, router, path);
				if (foundMenuItem) {
					return foundMenuItem;
				}
			}
		}
		return null;
	},
	refreshMenuItems(items, router, breadcrumbItems) {
		let $this = this;
		this.removeSelectedClass(items)
		for (let breadcrumbItem of breadcrumbItems) {
			
			let route = router.resolve(breadcrumbItem.to);
			const menuItem = items.map(it => $this.searchMenuItemByRoutePath(it, router, route.path)).find(it => it !== null);
			if (menuItem) {
				menuItem.selectedClass = selectedMenuItemClass;
				if (menuItem.parentMenuItem) {
					menuItem.parentMenuItem.selectedClass = parentSelectedMenuItemClass;
					menuItem.parentMenuItem.expanded = true;
				}
			}
			return;
		}
	}
} 
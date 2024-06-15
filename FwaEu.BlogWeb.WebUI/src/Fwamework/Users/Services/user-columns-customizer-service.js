const userListCustomizers = [];
export default {
	addUserListCustomizer(customizer) {
		userListCustomizers.push(customizer);
	},

	onColumnsCreated(pageComponent, columns) {
		userListCustomizers.forEach(customizer => {
			if (customizer.onColumnsCreated) {
				customizer.onColumnsCreated(pageComponent, columns);
			}
		});
		columns.forEach(function (col) {
			if (typeof col.allowSearch === "undefined")
				col.allowSearch = true;
		})
	}
}
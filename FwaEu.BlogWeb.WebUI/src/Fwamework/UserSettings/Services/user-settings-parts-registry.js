let userSettingsParts = [];

export default {
	addUserSettingsPart(userSettingsPart) {
		userSettingsParts.push(userSettingsPart);
	},
	getAll() {
		return userSettingsParts;
	}
}
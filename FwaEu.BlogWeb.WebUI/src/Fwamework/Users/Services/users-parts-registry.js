let userParts = [];

export default {
	addUserPart(userPart) {
		userParts.push(userPart);
	},
	getAll() {
		return userParts;
	}
}
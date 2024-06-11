import CurrentUserService from "@/Fwamework/Users/Services/current-user-service";

export default {
	getCurrentUserPermissionsAsync: async function () {
		let currentUser = await CurrentUserService.getAsync();
		return currentUser?.parts?.permissions?.selectedIds ?? [];
	}
};


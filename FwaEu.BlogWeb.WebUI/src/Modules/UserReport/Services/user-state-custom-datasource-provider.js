import UserStateService from '@/Fwamework/Users/Services/user-state-service';

export default {
	type: "UserState",
	async getDataSourceAsync(parameters) {
		const states = UserStateService.getAll();
		return states;
	}
}


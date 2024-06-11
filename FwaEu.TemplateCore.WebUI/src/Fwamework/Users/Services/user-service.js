import HttpService from '@/Fwamework/Core/Services/http-service';

export default {
	async getUserByIdAsync(userId)
	{
		if (!userId)
		{
			throw 'User id is required to get an user.';
		}

		let response = await HttpService.get(`Users/get/${userId}`);
		return response.data;
	},


	

	async getAllAsync() {
		return (await HttpService.get("Users/getAll")).data;
	},

	async saveAsync(id, user) {
		let response = await HttpService.put(`Users/save/${id}`, user);
		return response.data;
	}
}
import HttpService from '@/Fwamework/Core/Services/http-service';

export default {
	async saveAsync(id, user) {
		let response = await HttpService.put(`UserSettings/save/${id}`, user);//TODO: à changer quand webapi est fourni
		return response.data;
	}
}
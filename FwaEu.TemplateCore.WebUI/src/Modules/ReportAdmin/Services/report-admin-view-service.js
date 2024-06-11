import HttpService from "@/Fwamework/Core/Services/http-service";

export default {
	async saveAsync(invariantId, model) {
		let result = await HttpService.post(`Reports/Admin/${invariantId}`, model);
		return result.data;
	},
}


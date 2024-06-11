import HttpService from '@/Fwamework/Core/Services/http-service';

export default {
	async getAccessibleTasksAsync() {
		const response = await HttpService.get("UserTasks");
		return response.data;
	},

	async executeAsync(userTaskName, parameters) {
		const response = await HttpService.put(`UserTasks/Execute/${userTaskName}`, parameters || {});
		return response.data;
	}
}
import HttpService from '@/Fwamework/Core/Services/http-service';

export default {
	async executeSetupTaskAsync(taskName, taskArguments) {
		let response = await HttpService.put(`Setup/Execute/${taskName}`, taskArguments || {});
		return response.data;
	}
}
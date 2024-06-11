import HttpService from '@/Fwamework/Core/Services/http-service';

export default {
	async reinitializePasswordAsync(email) {
		return await HttpService.post('PasswordRecovery/ReinitializePassword', { email: email });
	},
	async updatePasswordAsync(userId, guid, password) {
		return await HttpService.post('PasswordRecovery/UpdatePassword', { userId: userId, guid: guid, newPassword: password });
	}

};


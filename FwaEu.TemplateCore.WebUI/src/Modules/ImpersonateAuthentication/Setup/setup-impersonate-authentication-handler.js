import { DefaultAuthenticationHandler } from '@/Modules/DefaultAuthentication/Services/default-authentication-handler';
import SetupService from '@/Fwamework/Setup/Services/setup-service';
import ImpersonateLoginSetupTask from '@/Modules/ImpersonateAuthentication/Components/impersonate-login-setup-task';

export const AuthenticationHandlerKey = 'SetupImpersonate'

class SetupImpersonateAuthenticationHandler extends DefaultAuthenticationHandler {
	constructor() {
		super(AuthenticationHandlerKey, 'SetupImpersonateCurrentUserToken');
	}

	async loginAsync(request) {
		const taskResult = await SetupService.executeSetupTaskAsync(ImpersonateLoginSetupTask.taskName, request);

		if (taskResult.data) {
			await this.authenticateWithTokenAsync(taskResult.data.token);
		}
		return taskResult;
	}

	createLoginComponentAsync = null;
}

export default new SetupImpersonateAuthenticationHandler();

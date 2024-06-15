import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';
import DefaultAuthenticationHandler from './Services/default-authentication-handler';

import UserPartsRegistry from '@/Fwamework/Users/Services/users-parts-registry';
import UserCredentialsPart from './UserParts/Credentials/credentials-user-part';
import UserPasswordRecoveryPart from './UserParts/PasswordRecovery/password-recovery-part';

import UserSettingsPartsRegistry from '@/Fwamework/UserSettings/Services/user-settings-parts-registry';
import UserSettingsPasswordPart from './UserParts/Credentials/user-settings-password-part';

export class DefaultAuthenticationModule extends AbstractModule {

	registerUserParts() {
		UserPartsRegistry.addUserPart(UserCredentialsPart);
		UserPartsRegistry.addUserPart(UserPasswordRecoveryPart);
	}

	registerUserSettingsParts() {
		UserSettingsPartsRegistry.addUserSettingsPart(UserSettingsPasswordPart);
	}

	async onInitAsync() {
		await DefaultAuthenticationHandler.configureAsync();
		this.registerUserParts();
		this.registerUserSettingsParts();
	}
}

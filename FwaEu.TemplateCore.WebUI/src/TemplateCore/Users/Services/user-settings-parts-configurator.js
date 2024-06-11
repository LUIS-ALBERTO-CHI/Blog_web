import CulturePart from '@/Modules/UserCulture/Components/culture-user-part';
import UserSettingsPartsRegistry from '@/Fwamework/UserSettings/Services/user-settings-parts-registry';

export default {

	configure() {
		UserSettingsPartsRegistry.addUserSettingsPart(CulturePart);
	}
}
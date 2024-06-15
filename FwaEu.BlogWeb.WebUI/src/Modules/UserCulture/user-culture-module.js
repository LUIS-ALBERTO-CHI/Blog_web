import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";


export class UserCultureModule extends AbstractModule {

	async onInitAsync() {
		const UserLanguageProviderService = (await import("@/Modules/UserCulture/Services/user-language-provider-service")).default;
		await UserLanguageProviderService.configureAsync();
	}
}

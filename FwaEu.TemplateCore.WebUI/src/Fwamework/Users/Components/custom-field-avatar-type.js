import UsersMasterDataService from "@/Modules/UserMasterData/Services/users-master-data-service";
import { createUserAvatar } from "@/Fwamework/Users/Services/user-avatar-helper";

export default {
	type: "avatar",
	async createCellCustomTemplateAsync(container, data, property) {

		if (data.value) {
			const masterDataUser = await UsersMasterDataService.getAsync(data.value);
			const componentInstance = createUserAvatar({ user: masterDataUser });
			data.component.on('disposing', function () {
				componentInstance.unmount();
			});
			container.appendChild(componentInstance.$el);
		}
	}
}

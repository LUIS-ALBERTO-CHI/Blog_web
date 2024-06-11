import UserGroupMasterDataService from "@/Modules/UserGroups/Services/user-groups-master-data-service"

export default {
	titleKey: 'userGroupsTitle',
	key: 'UserGroups',
	async fetchDataAsync() {
		return await UserGroupMasterDataService.getAllAsync();
	}
}
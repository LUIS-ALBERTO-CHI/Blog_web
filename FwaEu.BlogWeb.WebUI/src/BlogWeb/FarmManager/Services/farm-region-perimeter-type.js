import FarmRegionMasterDataService from "@/BlogWeb/FarmManager/Services/Regions/farm-regions-master-data-service";

export default {
	titleKey: 'regionsTitle',
	key: 'RegionPerimeters',
	async fetchDataAsync() {
		return await FarmRegionMasterDataService.getAllAsync();
	}
}
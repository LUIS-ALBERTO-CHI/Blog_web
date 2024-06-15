import UserHeaderService from '@/Modules/UserHeader/Services/user-header-service';
import FarmRegionHeaderService from '@/BlogWeb/FarmManager/Services/Regions/farm-region-header-service';
import SearchHeaderService from '@/Modules/Search/Services/search-header-service';
import PersistentNotificationHeaderService from '@/Modules/PersistentNotifications/Services/persistent-notifications-header-service';

export default {
	async configureAsync() {
		await SearchHeaderService.configureAsync();
		await FarmRegionHeaderService.configureAsync();
		await PersistentNotificationHeaderService.configureAsync();
		await UserHeaderService.configureAsync();
	}
}
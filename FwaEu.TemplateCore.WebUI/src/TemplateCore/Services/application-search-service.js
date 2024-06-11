import SearchService from '@/Modules/Search/Services/search-service';
import { UserIdSearchProvider, UserSearchProvider } from '@/Modules/UsersSearch/Services/users-search-provider';
import { FarmerSearchProvider, FarmIdSearchProvider, FarmSearchProvider } from '../FarmManager/Services/farms-search-provider';

export default {
	async configureAsync() {
		SearchService.register(new UserSearchProvider());
		SearchService.register(new UserIdSearchProvider());
		SearchService.register(new FarmSearchProvider());
		SearchService.register(new FarmIdSearchProvider());
		SearchService.register(new FarmerSearchProvider());
	}
}
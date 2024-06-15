import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';
import FarmTownsMasterDataService from './Services/farm-towns-master-data-service';
import FarmPostalCodeMasterDataService from './Services/farm-postal-code-master-data-service';
import FarmActivitiesMasterDataService from './Services/farm-activities-master-data-service';
import FarmRegionsMasterDataService from './Services/Regions/farm-regions-master-data-service';
import FarmAnimalSpeciesMasterDataService from './Services/farm-animal-species-master-data-service';
import FarmCategorySizesMasterDataService from './Services/farm-category-sizes-master-data-service';
import { hasPermissionAsync } from '@/Fwamework/Permissions/Services/current-user-permissions-service';
import { CanAccessToFarmManager } from './farms-permissions';
import UsersPartsRegistry from '@/Fwamework/Users/Services/users-parts-registry';
import UserSettingsPartsRegistry from '@/Fwamework/UserSettings/Services/user-settings-parts-registry';
import FarmerUserPart from './Services/farmer-user-part';

import { MenuCounterUserTaskItem } from '@/Modules/UserTasksNavigationMenu/Services/menu-counter-user-task-item';
import UserTasksManagerService from '@/Modules/UserTasks/Services/user-tasks-manager-service';
import FarmsWithoutAnimalsUserTask, { FarmsWitoutAnimalsTaskName } from './UserTasks/farms-without-animals-user-task';
import FarmerCountUserTask from './UserTasks/farmer-count-user-task';
import { I18n } from "@/Fwamework/Culture/Services/localization-service";

import GenericAdminConfigurationService from '@/Modules/GenericAdmin/Services/generic-admin-configuration-service';
import FarmTownConfiguration from '@/BlogWeb/FarmManager/Services/farm-town-configuration';
import FarmTownPostalCodeConfiguration from '@/BlogWeb/FarmManager/Services/farm-postal-code-configuration';
import FarmtActivityConfiguration from '@/BlogWeb/FarmManager/Services/farm-activity-configuration';
import FarmtAnimalSpeciesConfiguration from '@/BlogWeb/FarmManager/Services/farm-animal-species-configuration';

export class FarmManagerModule extends AbstractModule {

    async onInitAsync() {
		await FarmTownsMasterDataService.configureAsync();
		await FarmActivitiesMasterDataService.configureAsync();
		await FarmAnimalSpeciesMasterDataService.configureAsync();
		await FarmRegionsMasterDataService.configureAsync();
		await FarmCategorySizesMasterDataService.configureAsync();
		await FarmPostalCodeMasterDataService.configureAsync();
		UsersPartsRegistry.addUserPart(FarmerUserPart);
		UserSettingsPartsRegistry.addUserSettingsPart(FarmerUserPart);

		UserTasksManagerService.add(FarmsWithoutAnimalsUserTask);
		UserTasksManagerService.add(FarmerCountUserTask);

		GenericAdminConfigurationService.register(FarmTownConfiguration);
		GenericAdminConfigurationService.register(FarmtActivityConfiguration);
		GenericAdminConfigurationService.register(FarmtAnimalSpeciesConfiguration);
		GenericAdminConfigurationService.register(FarmTownPostalCodeConfiguration);
    }

	async getMenuItemsAsync(menuType) {
		let menuItems = [];
		if (menuType === "sideNavigation" && await hasPermissionAsync(CanAccessToFarmManager)) {
            menuItems.push({
                textKey: "farmsMenuItemText",
				path: { name: "Farms" },
				icon: "fa fa-farm",
				color: "#672bae",
				userTask: new MenuCounterUserTaskItem(FarmsWitoutAnimalsTaskName),
				menuActionOptions: {
					items: [{
						icon: 'fa-solid fa-plus',
						text: I18n.t('addFarm'),
						action: { name: "CreateFarm" }
					},
					{
						icon: 'fa-solid fa-globe',
						text: 'Map',
						action: () => { alert("Test action comme fonction") }
					}]
				}
            });
        }
        return menuItems;
    }
}
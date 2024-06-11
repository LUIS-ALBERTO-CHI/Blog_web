const GeneralInformationPage = () => import("@/TemplateCore/FarmManager/Components/GeneralInformationPageComponent.vue");
const FarmsPage = () => import("@/TemplateCore/FarmManager/Components/FarmsPageComponent.vue");
const SummaryPage = () => import("@/TemplateCore/FarmManager/Components/SummaryPageComponent.vue");
const AnimalsQuantitiesPage = () => import("@/TemplateCore/FarmManager/Components/AnimalsQuantitiesModificationPageComponent.vue");
import { CheckOperation } from "@/Fwamework/Permissions/Services/current-user-permissions-service";
import { CanAccessToFarmManager, CanSaveFarms, CanDeleteFarms } from "@/TemplateCore/FarmManager/farms-permissions";

export default [
	{
		path: '/Farms',
		name: 'Farms',
		component: FarmsPage,
		meta: {
			breadcrumb: {
				titleKey: 'farmsPage',
				parentName: 'default'
			},
			requiredPermissions: [CanAccessToFarmManager]
		}
	},
	{
		path: '/Farms/WithoutAnimals',
		name: 'FarmsWithoutAnimals',
		component: FarmsPage,
		meta: {
			breadcrumb: {
				titleKey: 'farmsWithoutAnimalsPage',
				parentName: 'Farms'
			},
			requiredPermissions: [CanAccessToFarmManager]
		},
		props: { onlyFarmsWithoutAnimals: true }
	},
	{
		path: '/Farms/Create',
		name: 'CreateFarm',
		component: GeneralInformationPage,
		meta: {
			breadcrumb: {
				titleKey: 'generalInformation',
				parentName: 'Farms'
			},
			requiredPermissions: [CanSaveFarms]
		}
	},
	{
		path: '/Farms/:id/GeneralInformation',
		name: 'FarmDetails',
		component: GeneralInformationPage,
		meta: {
			breadcrumb: {
				titleKey: 'generalInformation',
				parentName: 'FarmSummary'
			},
			requiredPermissions: [CanSaveFarms, CanDeleteFarms],
			requiredPermissionsCheckOperation: CheckOperation.Any
		}
	},
	{
		path: '/Farms/:id',
		name: 'FarmSummary',
		component: SummaryPage,
		meta: {
			breadcrumb: {
				parentName: 'Farms',
				async onNodeResolve(node, context) {
					if (typeof context.currentComponent.getCurrentFarmAsync !== "function") {
						throw new Error("Children pages of farm summary must implement a getCurrentFarmAsync method");
					}

					const currentFarm = await context.currentComponent.getCurrentFarmAsync();
					node.text = currentFarm.name;
					node.to = {
						name: 'FarmSummary',
						params: { id: currentFarm.id }
					};
					return node;
				}
			},
			requiredPermissions: [CanAccessToFarmManager]
		}
	},
	{
		path: '/Farms/:id/Quantities',
		name: 'AnimalsQuantities',
		component: AnimalsQuantitiesPage,
		meta: {
			breadcrumb: {
				titleKey: 'animalsQuantities',
				parentName: 'FarmSummary'
			},
			requiredPermissions: [CanSaveFarms, CanDeleteFarms],
			requiredPermissionsCheckOperation: CheckOperation.Any
		}
	}
]
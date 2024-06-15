<template>
	<page-container type="summary" v-if="animalCountDataSource && farmDetails">
		<toolbar :menu-items="menuItems" :menu-options="{forceMenuMode: true}">
			<div>
				<div :class="stateCssClass"></div>
				<!--TODO https://dev.azure.com/fwaeu/BlogWeb/_workitems/edit/5007-->
				<div class="farm-state-label">
					<h1>{{$t("state", {stateLabel: farmState}) }}</h1>
				</div>
			</div>
		</toolbar>
		<layout>
			<column>
				<general-information :farm-details="farmDetails" />
			</column>
			<column>
				<animal-count :data-source="animalCountDataSource"></animal-count>
			</column>
		</layout>
	</page-container>
</template>
<script>
	import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue";
	import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
	import Layout from '@/Modules/Layouts/Components/LayoutComponent.vue';
	import Column from '@/Modules/Layouts/Components/LayoutColumnComponent.vue';
	import GeneralInformation from "@/BlogWeb/FarmManager/Components/SummaryGeneralInformationComponent.vue";
	import { AsyncLazy } from '@/Fwamework/Core/Services/lazy-load';
	import AnimalCount from "@/BlogWeb/FarmManager/Components/SummaryAnimalsCountComponent.vue";
	import NotificationService from "@/Fwamework/Notifications/Services/notification-service";
	import DialogService from '@UILibrary/Modules/Dialog/Services/dialog-service';
	import Toolbar from "@/Fwamework/Toolbar/Components/ToolbarComponent.vue";
	import { hasPermissionAsync } from "@/Fwamework/Permissions/Services/current-user-permissions-service";
	import { CanDeleteFarms } from "@/BlogWeb/FarmManager/farms-permissions";
	import FarmGeneralInformationService from "@/BlogWeb/FarmManager/Services/farm-general-information-service";
	import FarmAnimalsCountService from "@/BlogWeb/FarmManager/Services/farm-animals-count-service";

	export default {
		components: {
			PageContainer,
			GeneralInformation,
			Layout,
			Column,
			AnimalCount,
			Toolbar
		},
		data() {
			let $this = this;
			return {
				farmDetails: null,
				farmLazy: new AsyncLazy(() => FarmGeneralInformationService.getAsync($this.$route.params.id)),
				menuItems: [],
				animalCountDataSource: null
			}
		},
		created: showLoadingPanel(async function () {
			await loadMessagesAsync(this, import.meta.glob('@/BlogWeb/FarmManager/Components/Content/summary-messages.*.json'));

			let $this = this;
			if (await hasPermissionAsync(CanDeleteFarms)) {
				this.menuItems = [{
					text: $this.$t('remove'),
					async action() {
						await $this.deleteFarmAsync();
						$this.$router.push({ name: 'Farms' });
					},
					icon: "trash"
				}];
			}


			await Promise.all([this.loadFarmDetailsAsync(), this.loadAnimalsCountAsync()]);
		}),
		methods: {
			async loadFarmDetailsAsync() {
				let farmInfo = await this.farmLazy.getValueAsync();
				if (!farmInfo) {
					NotificationService.showError(`Farm details corresponding to the id: ${this.$route.params.id} not found`);
					return;
				}
				this.farmDetails = farmInfo;
			},
			async loadAnimalsCountAsync() {
				let farmId = this.$route.params.id;
				this.animalCountDataSource = await FarmAnimalsCountService.getAllByFarmIdAsync(farmId);
			},
			async getCurrentFarmAsync() {
				return await this.farmLazy.getValueAsync();
			},
			async deleteFarmAsync() {
				let isDeleteConfirmed = await DialogService.confirmAsync(this.$t("askConfirmationForDelete"), this.$t("confirm"));
				if (isDeleteConfirmed) {
					await FarmGeneralInformationService.deleteAsync(this.farmDetails.id);
					NotificationService.showConfirmation(this.$t("farmDeleted"));
				}
			}
		},
		computed: {
			farmState() {
				return this.farmDetails?.closingDate ? this.$t("closed") : this.$t("opened");
			},
			stateCssClass() {
				return this.farmDetails?.closingDate ? "farm-state-closed" : "farm-state-opened";
			}
		}
	}
</script>
<style src="./Content/summary.css">
</style>
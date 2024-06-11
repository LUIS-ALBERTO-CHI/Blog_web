<template>
	<page-container type="form">
		<toolbar :menu-items="menuItems" :menu-options="{forceMenuMode: true}">
		</toolbar>
		<box>
			<dx-accordion :items="accordionData"
						 ref="accordionRef"
						 :collapsible="true"
						 :multiple="multiple">
				<template #title="{data}">
					{{ $t(data.key) }}
				</template>
				<template #item="{data}">
					<div>
						<div v-if="data.key === generalInformationKey">
							<general-information v-if="reportModel"
												 :ref="data.key"
												 v-model="reportModel"></general-information>
							<div class="form-buttons">
									
								<dx-button :text="$t('next')" @click="goToNextTabAsync(data)" type="default"></dx-button>
							</div>
						</div>
						<div v-if="data.key === dataAndFiltersKey">
							<data-and-filters v-if="reportModel"
											  :ref="data.key"
											  v-model="reportModel"
											  @datasource-changed="reportDataSourceChanged"></data-and-filters>
							<div class="form-buttons">
								<dx-button :text="$t('next')" @click="goToNextTabAsync(data)" type="default"></dx-button>
							</div>
						</div>
						<div v-if="data.key === loadDataSourceKey">
							<load-data-source v-if="reportModel"
											  :ref="data.key"
											  :data-source="adminLoadDataSourceParameters"
											  :allowAdding="true" />
							<div class="form-buttons">
								<dx-button :text="$t('next')" @click="goToNextTabAsync(data)" type="default"></dx-button>
							</div>
						</div>
						<div v-if="data.key === propertiesKey">
							<properties v-if="reportModel"
										:ref="data.key"
										:data-object="dataSourceProperties">
							</properties>
						</div>
					</div>
				</template>
			</dx-accordion>
			<div class="form-buttons">
				<dx-button :text="$t('save')" @click="saveAsync" type="success" :disabled="!isSaveEnabled"></dx-button>
			</div>
		</box>
	</page-container>
</template>

<script>
	if (!String.prototype.truncateLast) {
		String.prototype.truncateLast = function (find) {
			if (!this && this == '') return '';
			if (!find && find == '') return this.toString();
			let splitedByFind = this.split(find);
			if (splitedByFind.length <= 1) return this.toString();
			splitedByFind.pop();

			return splitedByFind.join(find);
		};
	}

	import DxAccordion from 'devextreme-vue/accordion';
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import DxButton from "devextreme-vue/button";
	import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue";
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
	import ReportDetailsService, { generalInformationKey, dataAndFiltersKey, loadDataSourceKey, propertiesKey } from '@/Modules/ReportAdmin/Services/details-service'
	import GeneralInformation from '@UILibrary/Modules/ReportAdmin/Components/GeneralInformationComponent.vue';
	import LoadDataSource from '@UILibrary/Modules/ReportAdmin/Components/LoadDataSourceComponent.vue';
	import Properties from '@UILibrary/Modules/ReportAdmin/Components/PropertiesComponent.vue';
	import DataAndFilters from '@UILibrary/Modules/ReportAdmin/Components/DataAndFiltersComponent.vue';
	import ReportAdminService from '@/Modules/ReportAdmin/Services/report-admin-service';
	import ReportDataSourceService from "@/Modules/Reports/Services/report-data-source-service";
	import { DefaultReportIcon } from '@/Modules/Reports/reports-module';
	import ReportFieldMasterDataService from "@/Modules/ReportMasterData/Services/report-field-master-data-service";
	import { AsyncLazy } from '@/Fwamework/Core/Services/lazy-load';
	import DetailsService from '@/Modules/ReportAdmin/Services/details-service';
	import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";
	import DialogService from '@UILibrary/Modules/Dialog/Services/dialog-service';
	import NotificationService from "@/Fwamework/Notifications/Services/notification-service";
	import Toolbar from "@/Fwamework/Toolbar/Components/ToolbarComponent.vue";
	import NavigationMenuService from '@/Fwamework/NavigationMenu/Services/navigation-menu-service';
	const accordionRef = "accordionRef";

	export default {
		components: {
			DxAccordion,
			DxButton,
			Box,
			PageContainer,
			GeneralInformation,
			LoadDataSource,
			Properties,
			DataAndFilters,
			Toolbar,
		},
		props: {
			invariantId: String,
			cloneInvariantId: String
		},
		data() {
			const $this = this;
			return {
				reportAdminLazy: new AsyncLazy(() => ReportAdminService.getReportAdminByInvariantIdAsync($this.invariantId ?? $this.cloneInvariantId)),
				generalInformationKey: generalInformationKey,
				dataAndFiltersKey: dataAndFiltersKey,
				loadDataSourceKey: loadDataSourceKey,
				propertiesKey: propertiesKey,
				multiple: true,
				isSaveEnabled: false,
				accordionData: ReportDetailsService.getAccordionItems(),
				reportModel: null,
				dataSourceProperties: [],
				propertiesAssociations: [],
				adminLoadDataSourceParameters: [],
				menuItems: [],
			};
		},
		mounted() {
			this.accordion = this.$refs[accordionRef];
		},
		created: showLoadingPanel(async function () {
			await loadMessagesAsync(this, import.meta.glob('@/Modules/ReportAdmin/Components/Content/details-messages.*.json'));

			await this.loadReportOrDefaultAsync();
			if (this.invariantId != null && this.cloneInvariantId == null) {
				let $this = this;
				this.menuItems = [{
					text: this.$t('delete'),
					async action() {
						await $this.deleteReportAsync($this.invariantId);
					},
					icon: "trash"
				}];
			}
			if (this.invariantId != null || this.cloneInvariantId != null) {
				this.refreshLoadDataSourceVisibility();
				this.expandAllAccordionItems();
			}
			else {
				this.multiple = false;
				this.collapseAllAccordionItemsFromIndex(0);
			}
		}),
		methods: {
			reportDataSourceChanged(typeChanged) {
				if (typeChanged)
					this.refreshLoadDataSourceVisibility();
				this.collapseAllAccordionItemsFromIndex(this.accordionData.find(x => x.key == this.dataAndFiltersKey).index);
			},
			refreshLoadDataSourceVisibility() {
				this.accordionData.find(x => x.key == this.loadDataSourceKey).visible =
					this.reportModel?.dataSource?.type != null &&
					ReportDataSourceService.get(this.reportModel.dataSource.type).useCustomParameters;
			},
			collapseAllAccordionItemsFromIndex(accordionIndex) {
				this.isSaveEnabled = false;
				for (let i = accordionIndex; i < this.accordion.items.length; i++) {
					this.accordion.items[accordionIndex].isFirstTimeValidated = false;
					this.collapseAndDisableAccordonItem(i, (i > 0 && i != accordionIndex));
				}
				this.expandAndEnableAccordonItem(accordionIndex);
			},
			expandAllAccordionItems() {
				this.multiple = true;
				this.isSaveEnabled = true;
				for (let i = 0; i < this.accordion.items.length; i++) {
					this.expandAndEnableAccordonItem(i);
				}
			},
			collapseAndDisableAccordonItem(accordionIndex, disable) {
				if (disable) this.accordion.items[accordionIndex].disabled = true;
				this.accordion.instance.collapseItem(accordionIndex);
			},
			expandAndEnableAccordonItem(accordionIndex) {
				this.accordion.items[accordionIndex].disabled = false;
				this.accordion.instance.expandItem(accordionIndex);
			},
			async goToNextTabAsync(item) {
				if (item.isFirstTimeValidated)
					this.accordion.instance.expandItem(item.index + 1);
				else {
					const componentRef = this.$refs[item.key]
					let isPanelValid = true;
					if (componentRef && componentRef.validate != undefined)
						isPanelValid = await ReportDetailsService.validatePanelAsync(item, componentRef);
					if (isPanelValid) {
						item.isFirstTimeValidated = false;
						if (item.index < this.accordion.items.length) {
							this.accordion.items[item.index + 1].disabled = false;
							this.accordion.instance.expandItem(item.index + 1);
						}
						if (!this.accordion.items[this.accordion.items.length - 1].disabled) {
							this.isSaveEnabled = true;
						}
					}
				}

				if (item.key == dataAndFiltersKey) {
					if (this.reportModel.dataSource && this.reportModel.dataSource.type &&
						ReportDataSourceService.get(this.reportModel.dataSource.type).useCustomParameters) {
						this.adminLoadDataSourceParameters = await ReportAdminService.createAdminLoadDataSourceParametersFromDataSourceAsync(this.reportModel);
					}
					else {
						await this.goToNextTabAsync(this.accordionData.find(x => x.key == loadDataSourceKey));
					}
				}
				if (item.key == loadDataSourceKey) {
					ReportAdminService.saveParameterValuesInLocalStorage(this.reportModel.invariantId, this.adminLoadDataSourceParameters);
					let reportDataSourceProperties = await ReportAdminService.getReportDataSourcePropertiesAsync(this.reportModel, this.adminLoadDataSourceParameters);
					let properties = [];
					this.reportModel.properties = [];
					let _propertiesAssociations = this.propertiesAssociations;
					const reportFields = await ReportFieldMasterDataService.getAllAsync();
					reportDataSourceProperties.forEach(function (reportDataSourceProperty) {
						let dataSourceProperty = { name: reportDataSourceProperty, fieldInvariantId: null };

						// on récupère les anciennes associations
						let previousPropertyAssociation = _propertiesAssociations
							.find(x => x.name == reportDataSourceProperty && x.fieldInvariantId != null);
						if (previousPropertyAssociation) {
							dataSourceProperty.fieldInvariantId = previousPropertyAssociation.fieldInvariantId;
						}

						// On associe les properties et les fields qui matchent
						if (dataSourceProperty.fieldInvariantId == null) {
							const reportDataSourcePropertyNoId = reportDataSourceProperty.truncateLast("Id");
							if (reportFields.some(rf => rf.invariantId == reportDataSourcePropertyNoId)) {
								dataSourceProperty.fieldInvariantId = reportDataSourcePropertyNoId;
								_propertiesAssociations.push(dataSourceProperty);
							}
						}

						properties.push(dataSourceProperty);

					});
					this.reportModel.properties = properties;
					this.dataSourceProperties = this.reportModel.properties;
					this.propertiesAssociations = this.dataSourceProperties;
				}
			},
			async loadReportOrDefaultAsync() {
				if (this.invariantId || this.cloneInvariantId) {
					this.reportModel = await this.reportAdminLazy.getValueAsync();
					if (this.cloneInvariantId)
						this.reportModel.invariantId = "";
					else
						this.reportModel.invariantId = this.invariantId;
				}
				else
					this.reportModel = DetailsService.getEmptyReportDataObject();

				if (this.reportModel && this.reportModel.properties) {
					this.dataSourceProperties = this.reportModel.properties;
					this.propertiesAssociations = this.dataSourceProperties;
				}
				if (this.reportModel.dataSource?.type != null) {
					const reportDataSourceType = ReportDataSourceService.get(this.reportModel.dataSource.type);
					if (reportDataSourceType.preFetchDataAsync)
						await reportDataSourceType.preFetchDataAsync(this.reportModel.dataSource);
				}


				if (!this.reportModel.icon) this.reportModel.icon = DefaultReportIcon;
				this.adminLoadDataSourceParameters = await ReportAdminService.createAdminLoadDataSourceParametersFromDataSourceAsync(this.reportModel);
			},
			async saveAsync() {
				const reportDataSourceType = ReportDataSourceService.get(this.reportModel.dataSource.type);
				if (!reportDataSourceType.usePreFilters) {
					this.reportModel.filters = [];
				}

				let isValid = true;
				const selfRefs = this.$refs;
				const selfaccordion = this.accordion.instance;
				const validateAccordion = async (accordion) => {
					let componentRef = selfRefs[accordion.key]
					if (componentRef && componentRef.validate != undefined) {
                    const isPanelValid = await ReportDetailsService.validatePanelAsync(accordion, componentRef);
						if (!isPanelValid)
							selfaccordion.expandItem(accordion.index);
						isValid &= isPanelValid;
					}
				};
				for (const accordion of this.accordionData) {
					await validateAccordion(accordion);
				}	
				if (isValid) {
					await ReportAdminService.saveAsync(this.reportModel.invariantId, this.reportModel);
					await NavigationMenuService.reloadMenuItemsAsync();
					this.$router.push({ name: 'ReportAdmin' });
				}
			},
			async deleteReportAsync(invariantId) {
				let isDeleteConfirmed = await DialogService.confirmAsync(this.$t("askConfirmationForDelete"), this.$t("confirm"));
				if (isDeleteConfirmed) {
					await ReportAdminService.deleteAsync(invariantId);
					await NavigationMenuService.reloadMenuItemsAsync();
					NotificationService.showConfirmation(this.$t("reportDeleted"));
					this.$router.push({ name: 'ReportAdmin' });
				}
			},
		},
	}
</script>
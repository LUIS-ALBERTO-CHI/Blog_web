<template>
	<page-container type="list">
		<box>
			<div class="pre-filters" v-if="adminLoadDataSourceParameters && adminLoadDataSourceParameters.length > 0">
				<load-data-source v-if="reportModel"
								  :data-source="adminLoadDataSourceParameters" />
				<div class="form-buttons">
					<dx-button type="default" :text="$t('loadDataSource')" @click="loadReportOrDefault" />
				</div>
			</div>
			<dx-accordion :items="viewAccordionItems"
						  ref="accordionRef"
						  :collapsible="true"
						  :multiple="false"
						  @content-ready="viewAccordionContentReady"
						  v-if="reportModel">
				<template #title="{data}">
					{{ $t(data.viewType) }}
				</template>
				<template #item="{data}">
					<div>
						<dx-tab-panel :items="data.tabs"
									  item-title-template="title"
									  item-template="itemTemplate">
							<template #title="{ data: data }">
								<span v-bind:class="{ 'default-view': data.view.isDefault }">
									{{getViewName(data)}}									
								</span>
							</template>
							<template #itemTemplate="{ data: data }">
								<div class="view-details">
									<toolbar v-bind:class="{ 'toolbar-hidden': data.isNew }"
											 :menu-items="createViewToolBarItems(data)"
											 :menu-options="{forceMenuMode: true}">
									</toolbar>
									<report-view :ref="data.refName"
														   :data-source="data"
														   :load-parameters="adminLoadDataSourceParameters"
														   :report="reportModel"
														   v-on:tab-adding="addNewTab(data.viewType)"
														   v-on:tab-is-default-changed="updateDefaultView(data)"
														   :invariant-id="invariantId">
									</report-view>
								</div>
							</template>
						</dx-tab-panel>
					</div>
				</template>
			</dx-accordion>
			<div class="form-buttons">
				<dx-button type="success" :text="$t('save')" @click="saveAsync" />
			</div>
		</box>
	</page-container>
</template>

<script>

	import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue";
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import LoadDataSource from '@UILibrary/Modules/ReportAdmin/Components/LoadDataSourceComponent.vue';
	import ReportAdminService from '@/Modules/ReportAdmin/Services/report-admin-service';
	import { AsyncLazy } from '@/Fwamework/Core/Services/lazy-load';
	import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";
	import DxButton from "devextreme-vue/button";
	import DxTabPanel from 'devextreme-vue/tab-panel';
	import DxAccordion from 'devextreme-vue/accordion';
	import ReportView from '@UILibrary/Modules/ReportAdmin/Components/ReportViewComponent.vue';
	import Toolbar from "@/Fwamework/Toolbar/Components/ToolbarComponent.vue";
	import ReportDisplayService from "@/Modules/ReportDisplay/Services/report-display-service";
	import ReportAdminViewService from '@/Modules/ReportAdmin/Services/report-admin-view-service'
	import LocalizationService from "@/Fwamework/Culture/Services/localization-service";

	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
	export default {
		components: {
			Box,
			PageContainer,
			DxButton,
			DxAccordion,
			LoadDataSource,
			DxTabPanel,
			ReportView,
			Toolbar,
		},
		data() {
			const $this = this;
			return {
				reportAdminLazy: new AsyncLazy(() => ReportAdminService.getReportAdminByInvariantIdAsync($this.invariantId)),
				reportModel: null,
				viewTypes: ReportDisplayService.getAll().map(x => x.type),
				viewAccordionItems: [],
				adminLoadDataSourceParameters: [],
			};
		},
		props: {
			invariantId: String,
		},
		created: showLoadingPanel(async function () {
			await loadMessagesAsync(this, import.meta.glob('@/Modules/ReportAdmin/Components/Content/report-views-page.*.json'));

			this.reportModel = await this.reportAdminLazy.getValueAsync();
			this.reportModel.invariantId = this.invariantId;
			this.adminLoadDataSourceParameters = await ReportAdminService.createAdminLoadDataSourceParametersFromDataSourceAsync(this.reportModel);
			this.loadReportOrDefault();
		}),
		methods: {
			getViewName(tab) {
				if (tab.isNew || !tab.view.name) return '+';
				return tab.view.name[LocalizationService.getCurrentLanguage()];
			},
			findAccordionItem(viewType) {
				return this.viewAccordionItems.find(x => x.viewType == viewType);
			},
			createViewToolBarItems(view) {
				const $this = this;
				return [{
					text: $this.$t('remove'),
					action() {
						$this.removeTab(view);
					},
					icon: "fal fa-trash-alt"
				}];
			},
			removeTab(view) {
				const tabs = this.findAccordionItem(view.viewType).tabs;
				const index = tabs.indexOf(view);
				tabs.splice(index, 1);
			},
			updateDefaultView(tab) {
				const $this = this;
				this.viewTypes.forEach(function (viewType) {
					$this.findAccordionItem(viewType).tabs.forEach(function (x) {
						if (x.view != tab.view)
							x.view.isDefault = false;
					});
				});

			},
			addNewTab(viewType) {
				const accordionTabs = this.findAccordionItem(viewType).tabs;
				this.viewAccordionItems.find(x => x.viewType == viewType).tabs
					.push(this.createEmptyTab(viewType, accordionTabs.length))
			},
			createEmptyTab(viewType, index) {
				return ReportAdminService.createEmptyViewAdminTab(viewType, index);
			},
			viewAccordionContentReady() {
				this.$refs.accordionRef.instance.expandItem(0);
			},
			loadReportOrDefault() {
				const $this = this;
				this.viewAccordionItems = this.viewTypes.map(function (viewType, index) {
					return {
						index: index,
						viewType: viewType,
						tabs: $this.createViewTabs(viewType),
					}
				});
			},
			createViewTabs(viewType) {
				const tabs = this.reportModel.defaultViews[viewType].map(function (x, i) {
					return {
						viewType: viewType,
						id: i,
						isNew: false,
						view: x,
						refName: `reportAdminViewTabs_${viewType}_${i}`,
					};
				});
				tabs.push(this.createEmptyTab(viewType, tabs.length));
				return tabs;
			},
			async saveAsync() {
				let selfViews = this.reportModel.defaultViews;
				const selfRefs = this.$refs;
				let isValid = true;
				this.viewAccordionItems.forEach(function (item) {
					if (isValid) {
						item.tabs.forEach(function (tab) {
							if (selfRefs[tab.refName]) {
								isValid = selfRefs[tab.refName].isValid();
								selfRefs[tab.refName].refreshReportModelView();
							}
						});
						selfViews[item.viewType] =
							item.tabs.filter(x => !x.isNew)
								.map(tab => tab.view);
					}
				});
				if (isValid) {
					await ReportAdminViewService.saveAsync(this.invariantId, this.reportModel);
					this.$router.push({ name: 'ReportAdmin' });
				}
			},
		},

	}

</script>

<style scoped>
	.pre-filters {
		width: 500px;
		margin-bottom: 30px;
	}

	.view-details {
		padding: 10px;
	}

	span.default-view {
		font-weight: bold;
	}

	.toolbar-hidden {
		visibility: hidden;
	}
</style>
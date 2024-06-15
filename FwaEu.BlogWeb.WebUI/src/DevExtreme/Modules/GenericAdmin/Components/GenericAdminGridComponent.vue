<template>
	<div class="data-grid" v-if="columns">
		<dx-data-grid ref="dataGridInstanceRefKey"
					  :data-source="customStore"
					  :columns="dataGridColumns"
					  :remote-operations="dataGridRemoteOperations"
					  @init-new-row="onInitNewRow"
					  @row-inserting="onRowInserting"
					  @row-inserted="onRowInserted"
					  @row-updating="onRowUpdating"
					  @row-updated="onRowUpdated"
					  @row-removing="onRowRemoving"
					  @row-removed="onRowRemoved"
					  @initialized="onInitialized"
					  @exporting="(e) => onExporting(e, exportFileName)">
			<dx-toolbar>
				<dx-toolbar-item name="addRowButton" location="before" />
				<dx-toolbar-item name="resetFilters" widget="dxButton" location="after"
								 :options="{icon: 'clearformat', onClick: clearFilter, hint: $t('genericAdminResetFilters')}" />
				<dx-toolbar-item name="exportButton" />
				<dx-toolbar-item name="searchPanel" />
			</dx-toolbar>
			<dx-sorting mode="multiple"></dx-sorting>
			<dx-search-panel :visible="true" :width="250" />
			<dx-export :enabled="true" />
			<dx-editing :allow-adding="allowAdding"
						:allow-updating="allowUpdating"
						:allow-deleting="allowDeleting"
						mode="form">
				<dx-texts :confirm-delete-message="confirmDeleteMessage" />
				<dx-form @content-ready="onContentReady" />
			</dx-editing>
			<dx-filter-row :visible="showFilterRow" />
		</dx-data-grid>
	</div>
</template>

<script type="text/javascript">
	import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
	import { DxDataGrid, DxSorting, DxEditing, DxFilterRow, DxTexts, DxExport, DxForm, DxSearchPanel, DxToolbar, DxItem as DxToolbarItem } from 'devextreme-vue/data-grid';
	import DxButton from 'devextreme/ui/button';
	import DataGridHelperService from '@UILibrary/Modules/GenericAdmin/Services/data-grid-helper-service';
	import GenericAdminConfigurationService from '@/Modules/GenericAdmin/Services/generic-admin-configuration-service';
	import { onExporting } from '@/DevExtreme/Extensions/Services/export-helper';

	// TODO: A supprimer losrqu'on aura développé https://fwaeu.visualstudio.com/BlogWeb/_workitems/edit/7351/
	import { DxTextArea } from 'devextreme-vue/text-area';

	export default {
		components: {
			// TODO: A supprimer losrqu'on aura développé https://fwaeu.visualstudio.com/BlogWeb/_workitems/edit/7351/
			DxTextArea,// eslint-disable-line
			DxDataGrid,
			DxSorting,
			DxEditing,
			DxFilterRow,
			DxTexts,
			DxExport,
			DxForm,
			DxSearchPanel,
			DxToolbar,
			DxToolbarItem
		},
		computed: {
			allowAdding() {
				return this.configuration && this.configuration.authorizedActions.allowCreate;
			},
			allowUpdating() {
				return this.configuration && this.configuration.authorizedActions.allowUpdate;
			},
			allowDeleting() {
				return this.configuration && this.configuration.authorizedActions.allowDelete;
			},
			dataGridInstance() {
				return this.$refs.dataGridInstanceRefKey.instance;
			},
			confirmDeleteMessageValue() {
				return this.confirmDeleteMessage;
			},
			dataGridColumns() {
				return this.columns?.concat({ type: "buttons", caption: this.$t('actions'), visibleIndex: 0 });
			},
			dataGridRemoteOperations() {
				return {
					paging: this.genericAdminConfiguration.dataSourceOptions.isPaginated,
					filtering: this.genericAdminConfiguration.dataSourceOptions.isPaginated,
					sorting: this.genericAdminConfiguration.dataSourceOptions.isPaginated
				};
			}
		},
		created: showLoadingPanel(async function () {
			await loadMessagesAsync(this, import.meta.glob('@/Modules/GenericAdmin/Content/generic-admin-common.*.json'));
			await this.genericAdminConfiguration.loadMessagesAsync(this);

			this.genericAdminConfiguration.onComponentCreated(this);

			this.configuration = await GenericAdminConfigurationService.getConfiguration(this.configurationKey,
				this.genericAdminConfiguration.getApiViewContext());
			this.genericAdminConfiguration.onConfigurationLoaded(this);

			this.genericAdminConfiguration.onInitDataSource(this.configuration);
			this.customStore = this.genericAdminConfiguration.createDataSource(this, () => this.$refs.dataGridInstanceRefKey);
			const columns = DataGridHelperService.createColumns(this, this.$i18n);
			await this.genericAdminConfiguration.onColumnsCreatingAsync(this, columns);
			this.columns = columns;

			this.exportFileName = this.genericAdminConfiguration.getExportFileNameKey();
		}),
		data() {
			return {
				columns: null,
				customStore: null,
				dataSource: null,
				confirmDeleteMessage: undefined,
				showFilterRow: true,

				exportFileName: null,

				configuration: null
			};
		},
		methods: {
			clearFilter() {
				this.dataGridInstance.clearFilter();
			},
			onContentReady: async function (e) {
				await this.$nextTick();

				const formButtons = e.element.parentElement.parentElement.querySelectorAll(".dx-button[role='button']");
				let saveButton = DxButton.getInstance(formButtons[0]); //HACK: DevExtreme provides no way to differentiate buttons... To Update if it does not match anymore
				let cancelButton = DxButton.getInstance(formButtons[1]);//HACK: DevExtreme provides no way to differentiate buttons... To Update if it does not match anymore

				saveButton.option("type", "success");
				cancelButton.option("type", "normal");
			},
			onInitNewRow: function ($event) {
				this.genericAdminConfiguration.onInitNewRow(this, $event);
			},
			onRowInserting: function ($event) {
				this.genericAdminConfiguration.onRowInserting(this, $event);
			},
			onRowInserted: function ($event) {
				this.genericAdminConfiguration.onRowInserted(this, $event);
			},
			onRowUpdating: function ($event) {
				this.genericAdminConfiguration.onRowUpdating(this, $event);
			},
			onRowUpdated: function ($event) {
				this.genericAdminConfiguration.onRowUpdated(this, $event);
			},
			onRowRemoving: function ($event) {
				this.genericAdminConfiguration.onRowRemoving(this, $event);
			},
			onRowRemoved: function ($event) {
				this.genericAdminConfiguration.onRowRemoved(this, $event);
			},
			onInitialized: function ($event) {
				this.genericAdminConfiguration.onInitialized(this, $event);
			},
			onExporting
		},
		props: {
			configurationKey: {
				type: String,
				required: true
			},
			genericAdminConfiguration: {
				type: Object,
				required: true
			}
		}
	}
</script>
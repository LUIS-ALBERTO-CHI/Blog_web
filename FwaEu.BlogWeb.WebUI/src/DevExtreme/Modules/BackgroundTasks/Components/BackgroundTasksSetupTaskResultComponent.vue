<template>
	<div>
		<box :title="$t('title')">
			<dx-form>
				<dx-item :label="{text:'Auto refresh'}"
						 editor-type="dxSwitch"
						 :editor-options="switchBoxEditorOptions"/>
			</dx-form>
			<dx-data-grid v-if="datasource"
						  :data-source="datasource"
						  width="100%"
						  @exporting="(e) => onExporting(e, 'BackgroundTasks')">
				<dx-column data-field="taskName"
						   data-type="date" />
				<dx-column data-field="startDate"
						   data-type="date"
						   :format="dateFormat" />
				<dx-column data-field="queueDate"
						   data-type="date"
						   :format="dateFormat"
						   :sort-index="0"
						   sort-order="desc" />
				<dx-column data-field="numberOfTasksBefore" />
				<dx-column data-field="endDate"
						   data-type="date"
						   :format="dateFormat"
						   :sort-index="1"
						   sort-order="desc" />
				<dx-column data-field="state" />
				<dx-search-panel :visible="true" :width="250" />
				<dx-export :enabled="true" />
				<dx-paging :page-size="20" />
				<dx-sorting mode="multiple" />
			</dx-data-grid>
		</box>

	</div>
</template>
<script>
	import Box from '@/Fwamework/Box/Components/BoxComponent.vue';

	import { DxDataGrid, DxPaging, DxExport, DxSearchPanel, DxSorting, DxColumn } from 'devextreme-vue/data-grid';
	import { DxForm, DxItem } from 'devextreme-vue/form';
	import { DxSwitch } from 'devextreme-vue/switch';
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
	import SetupService from '@/Fwamework/Setup/Services/setup-service';
	import BackgroundTaskSetupTaskService from '@/Modules/BackgroundTasks/Services/background-task-setup-task-service.js';
	import { showLoadingPanel } from '@/Fwamework/LoadingPanel/Services/loading-panel-service';
	import { onExporting } from '@/DevExtreme/Extensions/Services/export-helper';

	export default {
		components: {
			Box,
			DxDataGrid,
			DxPaging,
			DxExport,
			DxSearchPanel,
			DxSorting,
			DxColumn,
			//NOTE: Need to include switch because it is used by the dx-item
			// eslint-disable-next-line vue/no-unused-components
			DxSwitch,
			DxForm,
			DxItem,
		},
		props: {
			setupTask: Object
		},
		data() {
			return {
				taskResult: null,
				datasource: null,
				switchBoxEditorOptions: {
					onValueChanged: this.onAutoRefreshSwitchBoxValueChanged,
				},

				dateFormat: 'MM/dd/yyyy HH:mm:ss',

				interval: null,
			};
		},
		created: showLoadingPanel(async function () {
			await this.refreshDatasourceAsync();
			await loadMessagesAsync(this, import.meta.glob('@/Modules/BackgroundTasks/Content/background-tasks-setup-task-result-messages.*.json'));
		}),

		methods: {
			startInterval() {
				this.interval = setInterval(() => {
					this.refreshDatasourceAsync();
				}, 1000);
			},
			async refreshDatasourceAsync() {
				this.taskResult = await SetupService.executeSetupTaskAsync("BackgroundTasks", null);
				this.datasource = BackgroundTaskSetupTaskService.getDatagridDatasource(this.taskResult);
			},
			onAutoRefreshSwitchBoxValueChanged(e) {
				if (e.value) {
					this.startInterval();
				}
				else {
					clearInterval(this.interval);
					this.interval = null;
				}
			},
			onExporting
		},

		computed: {
		}
	}
</script>
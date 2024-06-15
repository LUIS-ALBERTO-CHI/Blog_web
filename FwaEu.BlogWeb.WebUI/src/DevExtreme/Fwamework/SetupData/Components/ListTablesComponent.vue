<template>
	<box :title="$t('title')">
		<dx-tree-list v-if="listTables"
					  ref="databaseTreeList"
					  :data-source="listTables"
					  :auto-expand-all="true"
					  items-expr="items"
					  data-structure="tree"
					  :height="600">
			<dx-filter-row :visible="true" />
			<dx-selection :recursive="true"
						  :allow-select-all="true"
						  mode="multiple" />
			<dx-column data-field="name" />
		</dx-tree-list>
		<dx-button @click="deleteAllTables" :text="$t('deleteAllTables')"></dx-button>
		<dx-button @click="deleteSelectedTables" :text="$t('deleteSelectedTables')"></dx-button>

		<process-result v-if="taskResult" :results="taskResult.results" />
	</box>
</template>
<script>
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import ProcessResult from '@UILibrary/Fwamework/ProcessResults/Components/ProcessResultComponent.vue';
	import { DxTreeList, DxSelection, DxColumn, DxFilterRow } from 'devextreme-vue/tree-list';
	import { DxButton } from 'devextreme-vue/button';
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
	import SetupService from '@/Fwamework/Setup/Services/setup-service';
	import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";

	export default {
		components: {
			Box,
			ProcessResult,
			DxTreeList,
			DxSelection,
			DxColumn,
			DxFilterRow,
			DxButton
		},
		props: {
			setupTask: Object
		},
		data() {
			return {
				listTables: null,
				taskResult: null,
			};
		},
		created: showLoadingPanel(async function () {
			this.listTables = await this.getTablesAsync();
			await loadMessagesAsync(this, import.meta.glob('@/Fwamework/SetupData/Components/Content/list-tables-messages.*.json'));
		}),
		methods: {
			deleteAllTables: showLoadingPanel(async function () {
				this.taskResult = await SetupService.executeSetupTaskAsync("DropDatabase", { action: "ApplyChangesOnDatabase" });
				this.listTables = await this.getTablesAsync();
			}),
			deleteSelectedTables: showLoadingPanel(async function () {
				let tables = this.getSelectedTables();
				this.taskResult = await SetupService.executeSetupTaskAsync("DeleteTables", { tablesByDatabase: tables });
				this.listTables = await this.getTablesAsync();
				this.$refs.databaseTreeList.instance.clearSelection();
			}),
			async getTablesAsync() {
				let result = await SetupService.executeSetupTaskAsync(this.setupTask.taskName, null);
				let listTables = Object.keys(result.data.tablesByDatabase).map(database => {
					return {
						name: database,
						items: result.data.tablesByDatabase[database].map(table => {
							return { name: table, database: database };
						})
					};
				})
				return listTables;
			},
			getSelectedTables() {
				let selectedTables = this.$refs.databaseTreeList.instance.getSelectedRowsData("all");
				let data = {};
				if (selectedTables.length > 0) {
					selectedTables.filter(item => item.parentId != 0).forEach(item => {
						if (!data[item.database]) {
							data[item.database] = [];
						}
						data[item.database].push(item.name);
					})
				}
				return data;
			}
		}
	}
</script>

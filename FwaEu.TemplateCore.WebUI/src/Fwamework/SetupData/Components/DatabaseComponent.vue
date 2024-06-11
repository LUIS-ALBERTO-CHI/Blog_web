<template>
	<box :title="$t('title')">
		<div>
			<a href="#" @click="executeBeforeUpdateSchemaAsync">{{ $t('beforeUpdateSchema') }}</a>
		</div>
		<div>
			<a href="#" @click="getUpdatedScriptAsync">{{ $t('getUpdatedScript') }}</a>
		</div>
		<div>
			<a href="#" @click="updateSchemaAsync">{{ $t('updateSchema') }}</a>
		</div>
		<div>
			<a href="#" @click="executeAfterUpdateSchemaAsync">{{ $t('afterUpdateSchema') }}</a>
		</div>
		<div v-if="hasConnectionStrings">
			<box :title="$t('connectionStrings')" class="block">
				<div v-for="data in connectionStringsTaskResult.data" :key="data.name">
					<div>{{data.name}}:&nbsp;{{data.connectionString}}</div>
				</div>
			</box>
		</div>
		<process-result v-if="hasContexts" :results="taskResult.results" />
	</box>
</template>
<script>
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
	import SetupService from '@/Fwamework/Setup/Services/setup-service';
	import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";
	import ProcessResult from '@UILibrary/Fwamework/ProcessResults/Components/ProcessResultComponent.vue';
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";

	export default {
		components: {
			ProcessResult,
			Box
		},
		props: {
			setupTask: Object
		},
		data() {
			return {
				taskResult: null,
				connectionStringsTaskResult: null
			};
		},
		created: showLoadingPanel(async function () {
			await this.loadConnectionStringsAsync();

			await loadMessagesAsync(this, import.meta.glob('@/Fwamework/SetupData/Components/Content/database-messages.*.json'));
		}),
		methods: {
			executeBeforeUpdateSchemaAsync: showLoadingPanel(async function () {
				this.taskResult = await SetupService.executeSetupTaskAsync("BeforeUpdateSchema", null);
			}),

			getUpdatedScriptAsync: showLoadingPanel(async function () {
				this.taskResult = await SetupService.executeSetupTaskAsync("UpdateSchema", { action: "GenerateSql" });
			}),

			updateSchemaAsync: showLoadingPanel(async function () {
				this.taskResult = await SetupService.executeSetupTaskAsync("UpdateSchema", { action: "ApplyChangesOnDatabase" });
			}),

			executeAfterUpdateSchemaAsync: showLoadingPanel(async function () {
				this.taskResult = await SetupService.executeSetupTaskAsync("AfterUpdateSchema", null);
			}),

			loadConnectionStringsAsync: showLoadingPanel(async function () {
				this.connectionStringsTaskResult = await SetupService.executeSetupTaskAsync("ConnectionStrings", null);
			})
		},
		computed: {
			hasContexts() {
				return this.taskResult?.results?.contexts?.length;
			},
			hasConnectionStrings() {
				return this.connectionStringsTaskResult?.data?.length;
			}
		}
	}
</script>
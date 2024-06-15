<template>
	<box :title="$t('title')">
		<dx-button @click="restartApplication" :disabled="restarting" id="button" :height="35">
			<template #default>
				<span>
					<dx-load-indicator :visible="restarting"
									   class="button-indicator" />
					<span class="dx-button-text">{{$t('btnRestart')}}</span>
				</span>
			</template>
		</dx-button>
		<dx-button @click="clearCachesAsync" :text="$t('btnClearCaches')" />
		<process-result v-show="taskResults.contexts.length" :results="taskResults">
		</process-result>
	</box>
</template>
<script>
	import DxButton from "devextreme-vue/button";
	import DxLoadIndicator from "devextreme-vue/load-indicator";
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
	import ProcessResult from '@UILibrary/Fwamework/ProcessResults/Components/ProcessResultComponent.vue';
	import SetupService from '@/Fwamework/Setup/Services/setup-service';
	import ServerMonitoringService from '@/Modules/ServerMonitoring/Services/server-monitoring-service';
	import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";

	export default {
		components: {
			Box,
			DxButton,
			DxLoadIndicator,
			ProcessResult
		},
		data() {
			return {
				restarting: false,
				applicationRequestCount: 0,
				maxApplicationRequestAttemps: 5,
				taskResults: {
					contexts: []
				}
			};
		},
		props: {
			setupTask: Object
		},
			async created() {
		await loadMessagesAsync(this, import.meta.glob('@/Modules/SetupServerManagement/Components/Content/application-management-messages.*.json'));
	    },
		methods: {
			clearCachesAsync: showLoadingPanel(async function () {
				let taskResult = await SetupService.executeSetupTaskAsync("ClearCaches");
				this.taskResults.contexts = taskResult.results.contexts;
			}),
			async restartApplication() {
				this.restarting = true;
				this.taskResults.contexts = [{ name: "Restarting application", entries: [] }];

				this.addResultEntry({ type: "info", content: "Requesting stop application" });
				try {
					await SetupService.executeSetupTaskAsync("StopApplication");
					this.addResultEntry({ type: "info", content: "Application stop request finished, waiting 2 seconds before ping it" });

					await new Promise((resolve) => { setTimeout(resolve, 2 * 1000); });//Wait for 2 seconds before ping the application
					await this.checkApplicationStartAsync();
				} catch (ex) {
					this.addResultEntry({ type: "error", content: "Error stoping application", details: this.getErrorDetails(ex) });
				}
				this.applicationRequestCount = 0;
				this.restarting = false;
			},

			async checkApplicationStartAsync() {
				this.applicationRequestCount++;
				this.addResultEntry({ type: "info", content: `Checking if application is alive (try count: ${this.applicationRequestCount})` });

				try {
					await ServerMonitoringService.pingAsync();
					this.addResultEntry({ type: "info", content: "Application is alive!" });

				} catch (ex) {
					if (this.applicationRequestCount === this.maxApplicationRequestAttemps) {
						this.addResultEntry({ type: "error", content: "Max application requests exceded", details: this.getErrorDetails(ex) });
					}
					else {
						this.addResultEntry({ type: "warning", content: "Pinging application failed, waiting 1 second for next try", details: this.getErrorDetails(ex) });
						await new Promise((resolve) => { setTimeout(resolve, 1 * 1000); });
						await this.checkApplicationStartAsync();
					}
				}
			},
			getErrorDetails(error) {

				let details = [error.message, error.stack];
				if (error.response) {
					details.push(error.response.data);
				}
				return details
			},
			addResultEntry(entry) {
				this.taskResults.contexts[0].entries.push(entry);
			}
		}
	}
</script>
<style>


	#button .button-indicator {
		height: 20px;
		width: 20px;
		display: inline-block;
		vertical-align: middle;
		margin-right: 5px;
	}
</style>
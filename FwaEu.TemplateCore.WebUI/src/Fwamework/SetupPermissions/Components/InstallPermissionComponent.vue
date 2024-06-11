<template>
	<box :title="$t('title')">
		<a href="#" @click="installModulePermissions"> {{$t('installModulePermissions')}}</a>
		<process-result v-if="taskResult" :results="taskResult.results">
		</process-result>
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
			Box,
			ProcessResult
		},
		props: {
			setupTask: Object
		},
		data() {
			return {
				taskResult: null
			};
		},
		async created() {
			await loadMessagesAsync(this, import.meta.glob('@/Fwamework/SetupPermissions/Components/Content/install-permission-messages.*.json'));
		},
		methods: {
			installModulePermissions: showLoadingPanel(async function () {
				this.taskResult = await SetupService.executeSetupTaskAsync(this.setupTask.taskName, null);
			})
		}
	}
</script>
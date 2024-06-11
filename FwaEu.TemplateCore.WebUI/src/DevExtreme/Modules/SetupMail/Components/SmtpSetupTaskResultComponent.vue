<template>
	<div>
		<box :title="$t('title')" v-if="taskResult">
			<div v-if="isSmtpEnabled">
				<div class="block">
					<dx-form :form-data="taskResult.data" :read-only="true">
						<dx-item :caption="$t('host')" data-field="host" :label="{text:$t('host')}" />
						<dx-item data-field="port" :label="{text:$t('port')}" />
						<dx-item data-field="userName" :label="{text:$t('userName')}" />
						<dx-item data-field="hasPassword" :label="{text:$t('password')}" template="hasPasswordTemplate">
						</dx-item>
						<template #hasPasswordTemplate="cellInfo">
							<dx-text-box :text="getPasswordText(cellInfo.data.editorOptions.value)" :read-only="true" />
						</template>
						<dx-item data-field="enableSsl" :label="{text:$t('enableSsl')}" />
						<dx-item data-field="fromAddress" :label="{text:$t('fromAddress')}" />
						<dx-item data-field="ignoreSSLCertificateValidation" :label="{text:$t('ignoreSSLCertificateValidation')}" />
					</dx-form>
				</div>
				<div class="block">
					<box :title="$t('titleSMTPTest')">
						<dx-form :form-data="smtpTestModel">
							<dx-item :label="{text:$t('emailAddress')}" data-field="recipientAddress" />
						</dx-form>
						<div class="form-buttons">
							<dx-button @click="onTestSmtpClick" :text="$t('btnTestSmtp')"></dx-button>
						</div>
						<process-result ref="smtpTestTaskResult" v-if="smptTestResult" :results="smptTestResult.results" />
					</box>
				</div>
			</div>
			<span v-else>
				{{$t('smtpIsNotEnabled')}}
			</span>
		</box>
	</div>
</template>
<script>
	import Box from '@/Fwamework/Box/Components/BoxComponent.vue';
	import ProcessResult from '@UILibrary/Fwamework/ProcessResults/Components/ProcessResultComponent.vue';
	import { DxForm, DxItem } from "devextreme-vue/form";
	import { DxButton } from 'devextreme-vue/button';
	import DxTextBox from 'devextreme-vue/text-box';
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
	import SetupService from '@/Fwamework/Setup/Services/setup-service';
	import { showLoadingPanel } from '@/Fwamework/LoadingPanel/Services/loading-panel-service';

	export default {
		components: {
			ProcessResult,
			Box,
			DxForm,
			DxItem,
			DxTextBox,
			DxButton
		},
		props: {
			setupTask: Object
		},
		data() {
			return {
				taskResult: null,
				smptTestResult: null,
				smtpTestModel: {
					recipientAddress: null
				}
			};
		},
		created: showLoadingPanel(async function () {
			await loadMessagesAsync(this, import.meta.glob('@/Modules/SetupMail/Content/smtp-setup-task-result-messages.*.json'));

			this.taskResult = await SetupService.executeSetupTaskAsync("SmtpOptions", null);
		}),

		methods: {
			onTestSmtpClick: showLoadingPanel(async function () {
				this.smptTestResult = await SetupService.executeSetupTaskAsync("SendMail", this.smtpTestModel);
			}),
			getPasswordText(hasPassword) {
				return hasPassword ? "**********" : this.$t("noPassword");
			}
		},

		computed: {
			isSmtpEnabled() {
				return this.taskResult?.data?.host;
			}
		}
	}
</script>
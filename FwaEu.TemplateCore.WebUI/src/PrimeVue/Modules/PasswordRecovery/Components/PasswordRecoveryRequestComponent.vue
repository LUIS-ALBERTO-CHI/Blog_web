<template>
	<div>
		<!--TODO: https://dev.azure.com/fwaeu/TemplateCore/_workitems/edit/10862-->
		<Button :label="$t('reinitializePasswordLink')" link @click="visible = true"></Button>
		<Dialog v-model:visible="visible"
				:header="$t('reinitializePasswordLink')"
				modal
				:style="{ width: '380px', height: 'auto' }">
			<FormBuilder ref="emailValidate" v-model="formData">
				<FormItem dataField="email"
						  :editorOptions="{placeholder: $t('email')}"
						  :validationRules="[{ type : 'required' }, { type: 'email: true' }]"/>
			</FormBuilder>
				<div class="form-buttons">
					<Button :label="$t('button')"
							 @click="reinitializePasswordAsync"/>
				</div>
		</Dialog>
	</div>
</template>
<script>
	import Button from "primevue/button";
	import NotificationService from '@/Fwamework/Notifications/Services/notification-service';
	import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";
	import PasswordRecoveryService from '@/Modules/DefaultAuthentication/UserParts/PasswordRecovery/Services/password-recovery-service';
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
	import FormBuilder from "@/PrimeVue/Modules/FormBuilder/Components/FormBuilderComponent.vue";
	import FormItem from "@/PrimeVue/Modules/FormBuilder/Components/FormItemComponent.vue";
	import Dialog from 'primevue/dialog';

	export default {
		components: {
			Button,
			FormBuilder,
			FormItem,
			Dialog
		},
		async created() {
			await loadMessagesAsync(this, import.meta.glob('@/Modules/DefaultAuthentication/UserParts/PasswordRecovery/Components/Content/password-recovery-request-messages.*.json'));
		},
		data() {
			return {
				formData: {
					email: "",
				},
				visible: false
			};
		},
		methods: {
			reinitializePasswordAsync: showLoadingPanel(async function () {
				if (!await this.$refs.emailValidate.validateForm()) {
					return;
				}
				await PasswordRecoveryService.reinitializePasswordAsync(this.formData.email);
				NotificationService.showConfirmation(this.$t('mailSent'));
				this.visible = false;
			})
		}
	}
</script>

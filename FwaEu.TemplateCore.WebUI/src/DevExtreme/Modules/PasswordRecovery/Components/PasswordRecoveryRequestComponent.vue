<template>
	<div>
		<a href="#" @click.prevent.stop="showReinitializePasswordPopup">{{$t('reinitializePasswordLink')}}</a>
		<dx-popup v-if="popupVisible" v-model:visible="popupVisible"
				  :close-on-outside-click="true"
				  :width="380"
				  height="auto"
				  title-template="popupTitle"
				  :show-title="true">
			<template #popupTitle>
				{{$t('reinitializePasswordLink')}}
			</template>
			<dx-validation-group>
				<dx-text-box :placeholder="$t('email')" v-model:value="email">
					<dx-validator>
						<dx-required-rule/>
						<dx-email-rule/>
					</dx-validator>
				</dx-text-box>
				<div class="form-buttons">
					<dx-button type="success"
							   :text="$t('button')"
							   @click="reinitializePasswordAsync" />
				</div>
			</dx-validation-group>
		</dx-popup>
	</div>
</template>
<script>
	import { DxPopup } from 'devextreme-vue/popup';
	import DxButton from "devextreme-vue/button";
	import DxTextBox from "devextreme-vue/text-box";
	import DxValidationGroup from "devextreme-vue/validation-group";
	import DxValidator, { DxRequiredRule, DxEmailRule } from "devextreme-vue/validator";
	import NotificationService from '@/Fwamework/Notifications/Services/notification-service';
	import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";
	import PasswordRecoveryService from '@/Modules/DefaultAuthentication/UserParts/PasswordRecovery/Services/password-recovery-service';
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";

	export default {
		components: {
			DxPopup,
			DxButton,
			DxTextBox,
			DxValidator,
			DxRequiredRule,
			DxEmailRule,
			DxValidationGroup
		},
		async created() {
			await loadMessagesAsync(this, import.meta.glob('@/Modules/DefaultAuthentication/UserParts/PasswordRecovery/Components/Content/password-recovery-request-messages.*.json'));
		},
		data() {
			return {
				popupVisible: false,
				email: ""
			};
		},
		methods: {
			showReinitializePasswordPopup() {
				this.popupVisible = true;
			},
			reinitializePasswordAsync: showLoadingPanel(async function (e) {
				if (!e.validationGroup.validate().isValid) {
					return;
				}
				await PasswordRecoveryService.reinitializePasswordAsync(this.email);
				NotificationService.showConfirmation(this.$t('mailSent'));
				this.popupVisible = false;
			})
		}
	}
</script>

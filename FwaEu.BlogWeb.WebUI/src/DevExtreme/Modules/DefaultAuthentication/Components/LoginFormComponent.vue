<template>
	<dx-validation-group>
		<div class="dx-field">
			<dx-text-box :placeholder="$t('login')"
						 width="100%"
						 v-model:value="identity"
						 @enter-key="onFormItemEntered">
				<dx-validator>
					<dx-required-rule :message="$t('messageLogin')" />
				</dx-validator>
			</dx-text-box>
		</div>
		<div class="dx-field">
			<dx-text-box :placeholder="$t('password')"
						 width="100%"
						 mode="password"
						 v-model:value="password"
						 @enter-key="onFormItemEntered">
				<dx-validator>
					<dx-required-rule :message="$t('messagePassword')" />
				</dx-validator>
			</dx-text-box>
		</div>
		<div class="login-button">
			<dx-button ref="loginButton"
					   type="default"
					   :text="$t('button')"
					   @click="onLoginClickAsync" />
		</div>
	</dx-validation-group>
</template>

<script>
	import DxButton from "devextreme-vue/button";
	import DxTextBox from "devextreme-vue/text-box";
	import DxValidationGroup from "devextreme-vue/validation-group";
	import DxValidator, { DxRequiredRule } from "devextreme-vue/validator";
	import NotificationService from '@/Fwamework/Notifications/Services/notification-service';
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
	import AuthenticationService from '@/Fwamework/Authentication/Services/authentication-service';
	import { AuthenticationHandlerKey } from '@/Modules/DefaultAuthentication/Services/default-authentication-handler';

	export default {
		data() {
			return {
				identity: "",
				password: "",
				
			};
		},
		async created() {
			await loadMessagesAsync(this, import.meta.glob('@/Modules/DefaultAuthentication/UserParts/Credentials/Components/Content/login-messages.*.json'));
		},
		methods: {
			onFormItemEntered() {
				this.$refs.loginButton.$el.click();
			},

			async onLoginClickAsync(e) {

				if (!e.validationGroup.validate().isValid) {
					return;
				}
				await AuthenticationService.loginAsync(AuthenticationHandlerKey, { identity: this.identity, password: this.password }).catch(error => {
					if (error.response?.status === 401) {
						NotificationService.showWarning(this.$t("loginErrorMessage"));
					} else {
						throw error;
					}
				});
			}
		},
		components: {
			DxButton,
			DxTextBox,
			DxValidator,
			DxRequiredRule,
			DxValidationGroup
		}
	};
</script>
<style type="text/css">
		.login-page .login-button {
			text-align: center;
		}
</style>
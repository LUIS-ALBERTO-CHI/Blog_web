<template>
	<div class="login-button">
		<dx-button ref="loginButton"
				   type="default"
				   :text="$t('button')"
				   @click="onLoginClickAsync" />
	</div>
</template>

<script>
	import DxButton from "devextreme-vue/button";
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
	import AuthenticationService from '@/Fwamework/Authentication/Services/authentication-service';
	import { AuthenticationHandlerKey } from '@/Modules/AzureADAuthentication/Services/azure-ad-authentication-handler';

	export default {
		async created() {
			await loadMessagesAsync(this, import.meta.glob('@/Modules/AzureADAuthentication/Components/Content/login-messages.*.json'));
		},
		methods: {
			async onLoginClickAsync(e) {
				await AuthenticationService.loginAsync(AuthenticationHandlerKey);
			}
		},
		components: {
			DxButton
		}
	};
</script>
<style type="text/css">
	.login-page .login-button {
		text-align: center;
	}
</style>
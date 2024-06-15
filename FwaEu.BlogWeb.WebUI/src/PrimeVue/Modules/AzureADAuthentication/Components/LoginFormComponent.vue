<template>
	<div class="login-button">
		<Button ref="loginButton"
				   :label="$t('button')"
				   @click="onLoginClickAsync" />
	</div>
</template>

<script>
	import Button from "primevue/button";
	import AuthenticationService from '@/Fwamework/Authentication/Services/authentication-service';
	import { AuthenticationHandlerKey } from '@/Modules/AzureADAuthentication/Services/azure-ad-authentication-handler';
    import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";

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
			Button
		}
	};
</script>
<style type="text/css">
	.login-page .login-button {
		text-align: center;
	}
</style>
<template>
	<page-container type="form">
		<box>
			<FormBuilder ref="passwordReset" v-model="user">
				<FormItem dataField="password"
						  editorType="Password"
						  :validationRules="[{type : 'required'}, {type: 'min: 4'}]"
						  :editorOptions="{placeholder:  $t('newPassword'), toggleMask: true}" />
				<FormItem :class="w-full"
						  dataField="confirmPassword"
						  editorType="Password"
						  :validationRules="[{type : 'required' }, {type: 'confirmed:@password'}]"
						  :editorOptions="{placeholder:  $t('confirmPassword'), feedback: false, toggleMask: true}" />
			</FormBuilder>
			<div class="form-buttons">
				<Button :label="$t('updatePassword')"
						@click="sendPasswordRecovery"
						ref="updatePasswordButton" />
			</div>
		</box>
	</page-container>
</template>
<script>
	import FormBuilder from "@/PrimeVue/Modules/FormBuilder/Components/FormBuilderComponent.vue";
	import FormItem from "@/PrimeVue/Modules/FormBuilder/Components/FormItemComponent.vue";
	import Button from 'primevue/button'
	import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue";
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
	import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";
	import PasswordRecoveryService from "@/Modules/DefaultAuthentication/UserParts/PasswordRecovery/Services/password-recovery-service";
	import { Configuration } from "@/Fwamework/Core/Services/configuration-service";
	import AuthenticationService from '@/Fwamework/Authentication/Services/authentication-service';

	export default {
		components: {
			FormBuilder,
			FormItem,
			PageContainer,
			Box,
			Button
		},
		props: {
			userId: Number,
			guid: String
		},
		data() {
			return {
				user: {
					password: '',
					confirmPassword: ''
				}
			}
		},
		async created() {
			await loadMessagesAsync(this, import.meta.glob('@/Modules/DefaultAuthentication/UserParts/PasswordRecovery/Components/Content/password-recovery-messages.*.json'));
		},
		methods: {
			onPasswordEntered() {
				this.$refs.updatePasswordButton.$el.click();
			},
			sendPasswordRecovery: showLoadingPanel(async function () {
				if (!await this.$refs.passwordReset.validateForm()) {
					return;
				}
				await PasswordRecoveryService.updatePasswordAsync(this.userId, this.guid, this.user.password, this.user.confirmPassword);
				await AuthenticationService.logoutAsync();
				window.location.href = Configuration.application.publicUrl;
			}),
		}
	}
</script>

<style scoped>
	.page-container .box {
		margin-top: 50px;
		max-width: 300px;
		margin: 50px auto 0;
	}
</style>

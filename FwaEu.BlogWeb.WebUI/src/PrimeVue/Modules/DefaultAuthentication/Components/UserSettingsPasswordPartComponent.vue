<template>
	<!--TODO: https://dev.azure.com/fwaeu/BlogWeb/_workitems/edit/10862-->
	<FormBuilder ref="passwordPart" :col-count="1" v-model="modelValue.data">
		<FormItem dataField="currentPassword"
				  editorType="Password"
				  :validationRules="[{type : 'required'}]"
				  :editorOptions="{placeholder:  $t('currentPassword'), feedback: false, toggleMask: true}" />
		<FormItem dataField="newPassword"
				  editorType="Password"
				  :validationRules="[{type : 'required'}, {type: 'min: 4'}]"
				  :editorOptions="{placeholder:  $t('newPassword'), toggleMask: true}" />
		<FormItem :class="w-full"
				  dataField="confirmPassword"
				  editorType="Password"
				  :validationRules="[{type : 'required' }, {type: 'confirmed:@newPassword'}]"
				  :editorOptions="{placeholder:  $t('confirmPassword'), feedback: false, toggleMask: true}" />
	</FormBuilder>
</template>
<script>
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
	import FormBuilder from "@/PrimeVue/Modules/FormBuilder/Components/FormBuilderComponent.vue";
	import FormItem from "@/PrimeVue/Modules/FormBuilder/Components/FormItemComponent.vue";

	export default {
		components: {
			FormBuilder,
			FormItem
		},
		props: {
			modelValue: {
				type: Object,
				required: true,
			}
		},
		async created() {
			await loadMessagesAsync(this, import.meta.glob('@/Modules/DefaultAuthentication/UserParts/Credentials/Components/Content/user-settings-password-part-messages.*.json'));
		},
		methods: {
			async validateAsync() {
				if (!this.isPasswordRequired())
					return true;
				let validatorResult = await this.$refs.passwordPart.validateForm();
				return validatorResult;
			},
			isPasswordRequired() {
				return this.modelValue.data.newPassword || this.modelValue.data.confirmPassword;
			}
		}
	}
</script>
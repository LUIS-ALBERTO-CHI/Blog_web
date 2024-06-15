<template>
	<div>
		<dx-form :form-data="modelValue.data"
				 ref="passwordPart">

			<dx-item data-field="currentPassword"
					 :editor-options="passwordOptions"
					 :label="{text: $t('currentPassword')}">
				<dx-required-rule />
			</dx-item>

			<dx-item data-field="newPassword"
					 :editor-options="passwordOptions"
					 :label="{text: $t('newPassword')}">
				<dx-required-rule />
			</dx-item>

			<dx-item data-field="confirmPassword"
					 :editor-options="passwordOptions"
					 :label="{text: $t('confirmPassword')}">
				<dx-required-rule />
				<dx-compare-rule :comparison-target="passwordComparison"
								 type="compare"
								 :message="$t('messageConfirmPassword')" />
			</dx-item>
		</dx-form>
	</div>
</template>
<script>
	import { DxForm, DxItem, DxRequiredRule, DxCompareRule } from 'devextreme-vue/form';
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";

	export default {
		components: {
			DxForm,
			DxItem,
			DxRequiredRule,
			DxCompareRule
		},
		props: {
			modelValue: {
				type: Object,
				required: true,
			}
		},
		data() {
			return {
				passwordOptions: {
					mode: 'password',
				}
			}
		},
		async created() {
			await loadMessagesAsync(this, import.meta.glob('@/Modules/DefaultAuthentication/UserParts/Credentials/Components/Content/user-settings-password-part-messages.*.json'));
		},
		methods: {
			passwordComparison() {
				return this.modelValue.data.newPassword;
			},
			
			validateAsync() {
				if (!this.isPasswordRequired())
					return true;
				let validatorResult = this.$refs.passwordPart.instance.validate();
				return validatorResult.isValid;
			},
			isPasswordRequired() {
				return this.modelValue.data.newPassword || this.modelValue.data.confirmPassword;
			}
		}
	}
</script>
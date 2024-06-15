<template>
	<dx-form v-if="formItems"
			 :form-data="user"
			 :col-count="2"
			 :items="formItemsToBind"
			 ref="generalInformation"
			 @field-data-changed="onFormFieldDataChanged">
	</dx-form>
</template>

<script>
	import { DxForm } from 'devextreme-vue/form';
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
	import UserStateService from '@/Fwamework/Users/Services/user-state-service';
	import DxSwitch from 'devextreme-vue/switch';

	export default {
		components: {
			//HACK: We need to register dxSwitch to display the dxSwitch field in the dxForm
			DxSwitch,// eslint-disable-line

			DxForm,
		},
		props: {
			user: {
				type: Object,
				required: true
			},
			currentUser: {
				type: Object,
				required: true
			},
			userPartsFormItems: {
				type: Array,
				required: true
			},
		},
		data()
		{
			const states = UserStateService.getAll();
			const isCurrentUser = this.user.id === this.currentUser.id;
			return {
				stateOptions: {
					items: states,
					displayExpr: 'text',
					valueExpr: 'value',
					disabled: isCurrentUser
				},
				formItems: []
			};
		},
		async created() {
			await loadMessagesAsync(this, import.meta.glob('@/Fwamework/Users/Components/Content/general-information-messages.*.json'));
		},
		computed:
		{
			formItemsToBind()
			{
				return this.formItems.concat(this.userPartsFormItems);
			}
		},
		methods: {
			validateAsync()
			{
				let validatorResult = this.$refs.generalInformation.instance.validate();
				return validatorResult.isValid;
			},
			onFormFieldDataChanged(e) {
				if (e.dataField === "isAdmin") {
					this.$emit("is-admin-changed", e.value);
				}
			}
		}
	}
</script>
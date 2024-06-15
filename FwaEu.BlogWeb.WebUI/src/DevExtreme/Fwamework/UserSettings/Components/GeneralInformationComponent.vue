<template>
	<dx-form v-if="userPartsFormItems"
			 :form-data="user"
			 :col-count="2"
			 :items="userPartsFormItems"
			 ref="generalInformation">
	</dx-form>
</template>

<script>
	import { DxForm } from 'devextreme-vue/form';
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

		methods: {
			fillUser(user)
			{
				
			},
			validateAsync()
			{
				let validatorResult = this.$refs.generalInformation.instance.validate();
				return validatorResult.isValid;
			}
		}
	}
</script>




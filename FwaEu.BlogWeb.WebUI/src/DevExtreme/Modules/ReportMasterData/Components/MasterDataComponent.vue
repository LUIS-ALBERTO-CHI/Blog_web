<template>
	<dx-select-box v-model="value"
				   :data-source="dataSource"
				   @value-changed="valueChanged"/>
</template>
<script>
	import DxSelectBox from 'devextreme-vue/select-box';
	import MasterDataManagerService from '@/Fwamework/MasterData/Services/master-data-manager-service';

	export default {
		components: {
			DxSelectBox
		},
		props: {
			modelValue: {
				required: true
			}
		},
		data() {
			return {
				dataSource: MasterDataManagerService.getAllMasterDataServices().map(m => m.configuration.masterDataKey),
				value: this.modelValue,
			};
		},
		methods: {
			valueChanged() {
				this.$emit('update:modelValue', this.value);
			},
		},
	}
</script>

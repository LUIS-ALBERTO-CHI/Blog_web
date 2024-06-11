<template>
	<div>
		<dx-select-box :data-source="preFetchedData.farmCategories"
					   value-expr="id"
					   display-expr="name"
					   v-model="value"
					   @value-changed="valueChanged" />
	</div>
</template>

<script>
	import DxSelectBox from 'devextreme-vue/select-box';
	export default {
		components: {
			DxSelectBox,
		},
		props: {
			preFetchedData: {
				type: Object,
			},
			modelValue: {
				required: true
			}
		},
		data() {
			return {
				value: this.modelValue != null ? parseInt(this.modelValue) : null,
			};
		},
		methods: {
			valueChanged() {
				this.$emit('update:modelValue', this.value);
			},
		},
	}
</script>

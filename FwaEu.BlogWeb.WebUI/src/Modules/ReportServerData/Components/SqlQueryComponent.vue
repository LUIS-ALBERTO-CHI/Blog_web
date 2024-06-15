<template>
	<div>
		<dx-text-area height="90"
					  v-model="value"
					  @value-changed="valueChanged">
		</dx-text-area>
	</div>
</template>

<script>
	import DxTextArea from 'devextreme-vue/text-area';

	export default {
		components: {
			DxTextArea,
		},
		props: {
			modelValue: {
				required: true
			}
		},
		data() {
			return {
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

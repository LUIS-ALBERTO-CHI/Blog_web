<template>
	<div>
		<dx-check-box text="getAnimalSpecies" v-model="value.getAnimalSpecies" @value-changed="valueChanged"></dx-check-box>
		<dx-check-box text="getActivities" v-model="value.getActivities" @value-changed="valueChanged"></dx-check-box>
	</div>
</template>
<script>

	import DxCheckBox from 'devextreme-vue/check-box';

	export default {
		components: {
			DxCheckBox,
		},
		props: {
			modelValue: {
				required: true
			}
		},
		data() {
			return {
				value: this.modelValue == null ?
					{ "getAnimalSpecies": true, "getActivities": false } :
					JSON.parse(this.modelValue),
			};
		},
		methods: {
			valueChanged() {
				this.$emit('input', JSON.stringify(this.value));
			},
		},
	}
</script>

<template>
	<dx-validation-group>
		<dx-select-box :data-source="languagesList"
					   @valueChanged="onValueChanged"
					   display-expr="name"
					   value-expr="code"
					   :value="selectedValue"
					   field-template="field"
					   item-template="item">
			<dx-validator v-if="isRequired">
				<dx-required-rule />
			</dx-validator>
			<template #field="{ data }">
				<field :field-data="data" />
			</template>
			<template #item="{ data }">
				<item :item-data="data" />
			</template>
		</dx-select-box>
	</dx-validation-group>
</template>
<script>
	import DxSelectBox from 'devextreme-vue/select-box';
	import Field from '@UILibrary/Extensions/Components/FieldWithImageTemplateComponent.vue';
	import Item from '@UILibrary/Extensions/Components/ItemWithImageTemplateComponent.vue';
	import { DxValidator, DxRequiredRule } from 'devextreme-vue/validator';
	import DxValidationGroup from 'devextreme-vue/validation-group';

	export default {
		components: {
			DxSelectBox,
			Field,
			Item,
			DxValidator,
			DxRequiredRule,
			DxValidationGroup
		},
		props: {
			languageValue: {
				type: String
			},
			languages: {
				type: Array
			},
			isRequired: {
				type: Boolean
			}
		},
		created() {
			this.selectedValue = this.languageValue;
		},
		data() {
			return {
				selectedValue: ''
			}
		},
		methods: {
			onValueChanged(e) {
				this.$emit('update:languageValue', e.value);
			}
		},
		watch: {
			languageValue: function (newVal) {
				this.selectedValue = newVal;
			}
		},
		computed: {
			languagesList(){
				const $this = this;
				return this.languages.map(lang => {
					return {
						code: lang.code,
						name: $this.$t(lang.code),
						imageSrc: lang.imageSrc
					};
				});
			}
		}
	}

</script>
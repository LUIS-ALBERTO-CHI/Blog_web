<template>
	<div class="localizable-string-editor">
		<div v-for="item in formItems">
			<div class="p-inputgroup flex-1">
				<span class="p-inputgroup-addon"><img :src="flagButtonOptions(item.dataField).icon" alt="Image"/></span>				
				<InputText 
					v-model="item.validationField.value" type="text" 
					@input="updateModelValue(item.dataField, $event)"
					:maxlength="maxLength"
					:class="{ 'error-input' : item.validationField.errorMessage }"
					/>
				<span v-if="item.validationField.errorMessage" class="p-inputgroup-addon span-error-input">
					<i class="fa fa-exclamation-circle fa-lg icon-error-input" aria-hidden="true"></i>
				</span>
			</div>
			&nbsp;
		</div>
	</div>
</template>
<script>
	import LocalizationService from "@/Fwamework/Culture/Services/localization-service";
	
	import InputText from 'primevue/inputtext';
	import Image from 'primevue/image';
	import { useForm, useField } from 'vee-validate';
	import { ref } from 'vue';

	export default {
		components: {
			InputText,
			Image
		},
		props: {
			validationType: {
				type: String,
				default: null,
				validator: function (value) {
					return ['oneAtLeast', 'all', 'particularLanguages'].includes(value);
				}
			},
			modelValue: {
				type: Object,
				default: () => {
					return {};
				}
			},
			particularLanguageCodes: {
				type: Array,
				default: () => {
					return [];
				}
			},
			maxLength: {
				type: Number,
				default: null
			}
		},
		setup(props, { emit }) {

			const isValid = (currentValue,languageCode) => {
				if (currentValue || (props.validationType == "particularLanguages" && !props.particularLanguageCodes.includes(languageCode)))
				 	return true;
				if (props.validationType === "oneAtLeast") {
					return Object.values(form.values).some(value => value);
				}
				return false;
			}

			const form = useForm({
				initialValues: props.modelValue
			});

			const formItems = ref([]);			
			const supportedLanguages = LocalizationService.getSupportedLanguages();

			for (let language of supportedLanguages) {
				formItems.value.push({
					dataField: language.code,
					validationField: useField(language.code, (currentValue) => isValid(currentValue,language.code)),
					icon: new URL(language.imageSrc, import.meta.url).href
				})
			}

			const updateModelValue = () => {
				emit('update:modelValue', form.values);
			};

			return {
				formItems, supportedLanguages, form,
				updateModelValue
			};
		},

		methods: {		
			flagButtonOptions(languageCode) {
				const languageInfo = this.supportedLanguages.find(sl => sl.code == languageCode);
				return {
					icon: languageInfo.imageSrc,
					hint: this.$t(languageInfo.code),
					activeStateEnabled: false,
					focusStateEnabled: false,
					hoverStateEnabled: false,
					stylingMode: 'text'
				};
			},			
			async validateAsync() {
				const result = await this.form.validate();
				return result.valid; 
			}
		}
	}

</script>
<style src="@/PrimeVue/Fwamework/Culture/Content/localizable-string-input.css"></style>
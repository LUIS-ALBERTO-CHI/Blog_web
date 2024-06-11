<template>
	<div>
		<dx-form v-if="formItems.length"
				 :form-data="formDataModel"
				 :col-count="1"
				 :items="formItems"
				 ref="localizeString"
				 validation-group="localizableStringValidationGroup">
			<template #itemTemplate="{data}">
				<div class="localizable-string-editor">
					<dx-text-box v-model="formDataModel[data.dataField]">
						<dx-text-box-button :options="flagButtonOptions(data.dataField)"
											name="flag"
											location="before" />
						<dx-validator validation-group="localizableStringValidationGroup">
							<dx-custom-rule :validation-callback="() => isValid(data.dataField)" />
							<dx-string-length-rule :max="maxLength" v-if="maxLength" />
						</dx-validator>
					</dx-text-box>
				</div>
			</template>
		</dx-form>
	</div>
</template>
<script>
	import { DxForm } from 'devextreme-vue/form';
	import LocalizationService from "@/Fwamework/Culture/Services/localization-service";
	import DxTextBox, { DxButton as DxTextBoxButton } from 'devextreme-vue/text-box';
	import { DxValidator, DxCustomRule, DxStringLengthRule } from 'devextreme-vue/validator';

	export default {
		components: {
			DxForm,
			DxTextBox,
			DxTextBoxButton,
			DxValidator,
			DxCustomRule,
			DxStringLengthRule
		},
		data() {
			return {
				formItems: [],
				formDataModel: {},
				codes: [],
				supportedLanguages: LocalizationService.getSupportedLanguages()

			}
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
		created() {
			for (let language of this.supportedLanguages) {
				this.formItems.push({
					dataField: language.code,
					label: { visible: false },
					template: "itemTemplate"
				})
				this.codes.push(language.code);
			}
			this.formDataModel = this.modelValue;
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

			isValid(languageCode) {				
				const currentValue = this.formDataModel[languageCode];
				if (currentValue || (this.validationType == "particularLanguages" && !this.particularLanguagesCodes.includes(languageCode)))
					return true;

				if (this.validationType === "oneAtLeast") {
					for (let code of this.codes) {
						if (code !== languageCode && this.formDataModel[code])
							return true;
					}
				}
				return false;
			},
			validateAsync() {
				return Promise.resolve(this.$refs.localizeString.instance.validate().isValid);
			}
		}
	}

</script>
<style src="@/DevExtreme/Fwamework/Culture/Content/localizable-string-input.css"></style>
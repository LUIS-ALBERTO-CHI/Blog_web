<template>
	<dx-form :col-count="2"
			 validation-group="generalInformation"
			 ref="generalInformationForm"
			 :form-data="dataObject">

		<dx-item :is-required="true"
				 data-field="invariantId"
				 editor-type="dxTextBox">
			<dx-label :text="$t('code')" />
		</dx-item>

		<dx-item :editor-options="reportCategorySelectBoxOptions"
				 data-field="categoryInvariantId"
				 editor-type="dxSelectBox">
			<dx-label :text="$t('category')" />
		</dx-item>

		<template #nameTemplate="{data}">
			<localizable-string-input ref="nameLocalizableString"
									  validation-type="all"
									  :value="data.editorOptions.value"/>
		</template>
		<template #descriptionTemplate="{data}">
			<localizable-string-input ref="descriptionLocalizableString"
									  validation-type="all"
									  :value="data.editorOptions.value"/>
		</template>

		<dx-item :is-required="true"
				 template="nameTemplate"
				 data-field="name">
			<dx-label :text="$t('name')" />
		</dx-item>

		<dx-item :is-required="true"
				 template="descriptionTemplate"
				 data-field="description">
			<dx-label :text="$t('description')" />
		</dx-item>

		<dx-item data-field="isAsync"
				 editor-type="dxCheckBox">
			<dx-label :text="$t('isAsync')" />
		</dx-item>

		<template #iconPickerTemplate>
			<div>
				<icon-picker v-model="dataObject.icon"></icon-picker>
				{{dataObject.icon}}
			</div>
		</template>

		<template #navigationTemplate="{data}">
			<div>
				<div class="general-information-navigation">
					<div class="dx-field-label">
						<dx-check-box :text="$t('reportList')"
									  v-model="data.editorOptions.value.menu.visible" />
					</div>
					<div class="dx-field general-information-navigation number-box-right">
						<div class="dx-field-label">Index</div>
						<div class="dx-field-value">
							<dx-number-box :show-spin-buttons="true"
										   :min="0"
										   :max="100"
										   :disabled="!data.editorOptions.value.menu.visible"
										   width="6em"
										   v-model="data.editorOptions.value.menu.index" />
						</div>
					</div>
				</div>
				<div class="general-information-navigation">
					<div class="dx-field-label">
						<dx-check-box :text="$t('mainMenu')"
									  v-model="data.editorOptions.value.summary.visible" />
					</div>
					<div class="dx-field general-information-navigation number-box-right">
						<div class="dx-field-label">Index</div>
						<div class="dx-field-value">
							<dx-number-box :show-spin-buttons="true"
										   :min="0"
										   :max="100"
										   :disabled="!data.editorOptions.value.summary.visible"
										   width="6em"
										   v-model="data.editorOptions.value.summary.index" />
						</div>
					</div>
				</div>
			</div>
		</template>

		<dx-item template="iconPickerTemplate"
				 data-field="icon">
			<dx-label :text="$t('icon')" />
		</dx-item>

		<dx-item template="navigationTemplate"
				 data-field="navigation">
			<dx-label :text="$t('visibleOn')" />
		</dx-item>

	</dx-form>
</template>

<script>
	import { DxForm, DxItem, DxLabel } from "devextreme-vue/form"
	import DxCheckBox from "devextreme-vue/check-box"
	import DxNumberBox from "devextreme-vue/number-box"
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
	import LocalizableStringInput from "@UILibrary/Fwamework/Culture/Components/LocalizableStringInputComponent.vue";
	import { ReportCategoriesDataSourceOptions } from "@/Modules/ReportMasterData/Services/report-category-master-data-service";
	import IconPicker from "@UILibrary/Modules/FontAwesome/Components/IconPickerComponent.vue";

	export default {
		components: {
			DxForm,
			DxItem,
			DxLabel,
			DxCheckBox,
			DxNumberBox,
			LocalizableStringInput,
			IconPicker,
		},
		props: {
			modelValue: {
				type: Object,
				required: true
			}
		},
		data() {
			return {
				dataObject: Object.assign({}, this.modelValue),
				editorOptions: {
					numberBox: { min: 0, max: 100, showSpinButtons: true },
				},
				reportCategorySelectBoxOptions: {
					valueExpr: "invariantId",
					displayExpr: "name",
					dataSource: ReportCategoriesDataSourceOptions
				}
			};
		},
		async created() {
			await loadMessagesAsync(this, import.meta.glob('@/Modules/ReportAdmin/Components/Content/general-information-messages.*.json'));
		},
		methods: {
			async validateAsync() {
				let validateForm = this.$refs.generalInformationForm.instance.validate().isValid				
				let nameValidateFormLocalizable = await this.$refs.nameLocalizableString.validateAsync() 
            let descriptionValidateFormLocalizable = await this.$refs.descriptionLocalizableString.validateAsync() 

				return validateForm && nameValidateFormLocalizable && descriptionValidateFormLocalizable
				
			},
		}
	}

</script>

<style src="@/Modules/ReportAdmin/Content/report-admin.css"></style>
 
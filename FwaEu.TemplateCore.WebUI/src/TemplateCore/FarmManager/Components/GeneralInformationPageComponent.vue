<template>
	<page-container type="form">
		<box v-if="farmModel">
			<dx-form  ref="farmForm" :form-data="farmModel" :col-count="2">
				<dx-item data-field="name" :label="{text:$t('name')}">
					<dx-required-rule />
				</dx-item>
				<dx-item data-field="postalCodeId" :label="{text:$t('postalCode')}" editor-type="dxSelectBox" :editor-options="postalCodeSelectBoxOptions">
					<dx-required-rule />
				</dx-item>
				<dx-item data-field="categorySize" :label="{text:$t('categorySize')}" editor-type="dxSelectBox" :editor-options="sizeSelectBoxOptions">
					<dx-required-rule />
				</dx-item>
				<dx-item data-field="mainActivityId" :label="{text:$t('principalActivity')}" editor-type="dxSelectBox"
						 :editor-options="farmActivitySelectBoxOptions">
					<dx-required-rule />
				</dx-item>
				<dx-item data-field="sellingPriceInEurosWithoutTaxes" :label="{text:$t('sellingPrice')}" editor-type="dxNumberBox">
				</dx-item>
				<dx-item data-field="recruitEmployees" :label="{text:$t('employeesRecruitement')}" editor-type="dxSwitch"></dx-item>
				<dx-item data-field="openingDate" :label="{text:$t('openingDate')}" editor-type="dxDateBox">
					<dx-required-rule />
				</dx-item>
				<dx-item data-field="closingDate" :label="{text:$t('closingDate')}" editor-type="dxDateBox">
				</dx-item>
				<dx-item data-field="comments" :label="{text:$t('comment')}" editor-type="dxTextArea" :col-span="2" :editor-options="{height: 200}">
				</dx-item>
			</dx-form>
			<div class="form-buttons">
				<dx-button :text="$t('save')" @click="saveOrUpdateFarmInformationAsync" type="success" />
			</div>
		</box>
	</page-container>
</template>
<script>
	import { DxForm, DxItem, DxRequiredRule } from 'devextreme-vue/form';
	import { DxTextArea } from 'devextreme-vue/text-area';
	import DxSwitch from 'devextreme-vue/switch'
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";
	import { DxButton } from 'devextreme-vue/button';
	import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue";
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
	import { FarmPostalCodeDataSourceOptions } from '../Services/farm-postal-code-master-data-service';
	import { FarmActivitiesDataSourceOptions } from '../Services/farm-activities-master-data-service';	
	import { FarmCategorySizeDataSourceOptions } from "@/TemplateCore/FarmManager/Services/farm-category-sizes-master-data-service";
	import { AsyncLazy } from '@/Fwamework/Core/Services/lazy-load';
	import FarmGeneralInformationService from "@/TemplateCore/FarmManager/Services/farm-general-information-service";

	export default {
		components: {
			//HACK: We need to register dxTextArea and dxSwitch to display the text area and switch field in the dxForm
			DxTextArea,// eslint-disable-line
			DxSwitch,// eslint-disable-line

			DxForm,
			DxItem,
			DxButton,
			PageContainer,
			Box,
			DxRequiredRule
		},
		data() {
			let $this = this;
			return {
				farmModel: null,
				postalCodeSelectBoxOptions: {
					displayExpr: function (postalCode) {
						return postalCode ? `${postalCode.postalCode} - ${postalCode.townName}` : "";
					},
					dataSource: FarmPostalCodeDataSourceOptions,
					valueExpr: 'id'
				},
				farmActivitySelectBoxOptions: {
					dataSource: FarmActivitiesDataSourceOptions,
					displayExpr: 'name',
					valueExpr: 'id',
				},
				sizeSelectBoxOptions: {
					dataSource: FarmCategorySizeDataSourceOptions,
					displayExpr: 'text',
					valueExpr: 'id',
					showClearButton: true
				},
				farmLazy: new AsyncLazy(() => FarmGeneralInformationService.getAsync($this.$route.params.id)),
			}
		},
		created: showLoadingPanel(async function () {
			await loadMessagesAsync(this, import.meta.glob('@/TemplateCore/FarmManager/Components/Content/general-information-messages.*.json'));

			await this.loadFarmAsync();

		}),
		methods: {
			async saveOrUpdateFarmInformationAsync() {
				if (this.$refs.farmForm.instance.validate().isValid) {
					let farmId = this.$route.params.id;
					if (!this.isNew) {
						await FarmGeneralInformationService.updateAsync(farmId, this.farmModel);
					}
					else {
						let result = await FarmGeneralInformationService.saveAsync(this.farmModel);
						farmId = result.id;
					}
					this.$router.push({ name: 'FarmSummary', params: { id: farmId } });
				}
			},
			async loadFarmAsync() {
				this.farmModel = this.isNew ? { recruitEmployees: false } : (await this.farmLazy.getValueAsync());
			},
			async getCurrentFarmAsync() {
				return await this.farmLazy.getValueAsync();
			}
		},
		computed: {
			isNew() {
				return !this.$route.params.id;
			}
		}
	}
</script>
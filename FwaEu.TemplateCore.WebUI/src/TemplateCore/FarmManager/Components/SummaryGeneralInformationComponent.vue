<template>
	<box v-if="farmDetails" :title="$t('generalInformation')" :timeline="timeline" :menu-items="menuButtons">
		<dx-form :form-data="farmDetails" :read-only="true" :col-count="2">
			<dx-item data-field="name" :label="{text:$t('name')}" />
			<dx-item data-field="postalCodeId" :label="{text:$t('postalCode')}" editor-type="dxSelectBox" :editor-options="postalCodeSelectBoxOptions" />
			<dx-item data-field="categorySize" :label="{text:$t('categorySize')}" editor-type="dxSelectBox" :editor-options="sizeSelectBoxOptions" />
			<dx-item data-field="mainActivityId" :label="{text:$t('principalActivity')}" editor-type="dxSelectBox" :editor-options="farmActivitySelectBoxOptions" />
			<dx-item data-field="sellingPriceInEurosWithoutTaxes" :label="{text:$t('sellingPrice')}" />
			<dx-item data-field="recruitEmployees" :label="{text:$t('employeesRecruitement')}" editor-type="dxSwitch" />
			<dx-item data-field="openingDate" :label="{text:$t('openingDate')}" editor-type="dxDateBox" />
			<dx-item data-field="closingDate" :label="{text:$t('closingDate')}" editor-type="dxDateBox" :editor-options="{placeholder : $t('nullClosingDateText')}" />
			<dx-item data-field="comments" :label="{text:$t('comment')}" editor-type="dxTextArea" :col-span="2" :editor-options="{height: 200}" />
		</dx-form>
	</box>
</template>
<script>
	import { DxForm, DxItem } from 'devextreme-vue/form';
	import TimelineService from '@/Fwamework/Users/Services/timeline-service';
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
	import DxSwitch from 'devextreme-vue/switch'
	import { DxTextArea } from 'devextreme-vue/text-area';
	import { FarmPostalCodeDataSourceOptions } from '../Services/farm-postal-code-master-data-service';
	import { FarmActivitiesDataSourceOptions } from '../Services/farm-activities-master-data-service';
	import { FarmCategorySizeDataSourceOptions } from "@/TemplateCore/FarmManager/Services/farm-category-sizes-master-data-service";
	import { hasPermissionAsync } from '@/Fwamework/Permissions/Services/current-user-permissions-service';
	import { CanSaveFarms } from '@/TemplateCore/FarmManager/farms-permissions';


	export default {
		components: {
			//HACK: We need to register dxTextArea and dxSwitch to display the text area and switch field in the dxForm
			DxTextArea,// eslint-disable-line
			DxSwitch,// eslint-disable-line

			DxForm,
			DxItem,
			Box
		},
		props: {
			farmDetails: {
				type: Object,
				default: () => { }
			}
		},
		data() {
			return {
				timeline: null,
				menuButtons: [],
				sizePlaceholder: null,
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
					valueExpr: 'id'
				},
			}
		},
		async created() {
			await loadMessagesAsync(this, import.meta.glob('@/TemplateCore/FarmManager/Components/Content/summary-general-information-messages.*.json'));

			this.createTimeline();

			let $this = this;
			if (await hasPermissionAsync(CanSaveFarms)) {

				this.menuButtons = [{
					text: $this.$t('farmDetailsEdit'),
					action: () => {
						$this.$router.push({ name: 'FarmDetails', params: { id: $this.farmDetails.id } });
					},
					icon: "edit"
				}];
			}
			if (this.farmDetails && !this.farmDetails.categorySize) {
				this.sizeSelectBoxOptions.placeholder = this.$t("unspecified");
			}
		},
		methods: {
			createTimeline() {
				this.timeline = TimelineService.createCreatedAndUpdatedModels(
					{ date: this.farmDetails.createdOn, userId: this.farmDetails.createdById },
					{ date: this.farmDetails.updatedOn, userId: this.farmDetails.updatedById });
			},

		}
	}
</script>
<template>
	<page-container type="form">
		<box v-if="user" :menu-items="menuItems" :timeline="timelineModel">
			<general-information v-if="userPartsFormItems && userPartsFormItems.length > 0" ref="generalInformation"
				:user="user" :userPartsFormItems="userPartsFormItems" :current-user="currentUser"
				@is-admin-changed="onIsAdminChangedAsync" />
			<div class="block" v-if="visibleModelsWhichAreComponents && visibleModelsWhichAreComponents.length > 0">
				<dx-accordion :data-source="visibleModelsWhichAreComponents" :collapsible="true" :deferRendering="false"
					:selectedIndex="-1" selection-mode="none" ref="accordion">
					<template #title="tabPanelModel">
						<div>
							{{ tabPanelModel.data.title }}
						</div>
					</template>
					<template #item="tabPanelModel">
						<div>
							<component :ref="tabPanelModel.data.ref" :is="tabPanelModel.data.handler.createComponent()"
								v-model="tabPanelModel.data" :fetched-data="tabPanelModel.data.fetchedData" />
						</div>
					</template>
				</dx-accordion>
			</div>
			<div class="form-buttons">
				<dx-button :text="$t('save')" @click="saveAsync" type="success" />
			</div>
		</box>
	</page-container>
</template>
<script>
import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
import { DxAccordion } from 'devextreme-vue/accordion';
import GeneralInformation from '@UILibrary/Fwamework/Users/Components/GeneralInformationComponent.vue';
import PageContainer from '@/Fwamework/PageContainer/Components/PageContainerComponent.vue';
import { DxButton } from 'devextreme-vue/button';
import DxCheckBox from 'devextreme-vue/check-box';
import UserEditPageMixins from '@/Fwamework/Users/Services/user-edit-page-mixins';
import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
import UserDetailsMixins from '@/Fwamework/Users/Services/user-details-mixins';
export default {
	components: {
		Box,
		DxAccordion,
		GeneralInformation,
		PageContainer,
		DxButton,
		DxCheckBox
	},
	mixins: [UserEditPageMixins, UserDetailsMixins],
	async created() {
		await loadMessagesAsync(this, import.meta.glob('@/Fwamework/Users/Components/Content/user-details-messages.*.json'));
	}
}
</script>

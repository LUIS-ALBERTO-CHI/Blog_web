<template>
	<page-container type="form">
		<box v-if="currentUser && modelData.models" :title="$t('generalInformation')">
			<general-information v-if="userPartsFormItems && userPartsFormItems.length > 0" ref="generalInformation"
				:user="user" :userPartsFormItems="userPartsFormItems" :current-user="currentUser" />
			<div class="block" v-if="visibleModelsWhichAreComponents && visibleModelsWhichAreComponents.length > 0">
				<dx-accordion :data-source="visibleModelsWhichAreComponents" :collapsible="true" :multiple="true"
					:deferRendering="false" :selectedIndex="-1" ref="accordion">
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
import { DxButton } from 'devextreme-vue/button';
import { DxAccordion } from 'devextreme-vue/accordion';
import PageContainer from '@/Fwamework/PageContainer/Components/PageContainerComponent.vue';
import UserEditPageMixins from '@/Fwamework/Users/Services/user-edit-page-mixins';
import GeneralInformation from '@UILibrary/Fwamework/UserSettings/Components/GeneralInformationComponent.vue';
import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
import UserSettingsMixins from '@/Fwamework/Users/Services/user-settings-mixins';

export default {
	components: {
		Box,
		DxAccordion,
		PageContainer,
		DxButton,
		GeneralInformation
	},
	mixins: [UserEditPageMixins, UserSettingsMixins],
	async created() {
		await loadMessagesAsync(this, import.meta.glob('@/Fwamework/UserSettings/Components/Content/user-settings-messages.*.json'));
	},
}
</script>

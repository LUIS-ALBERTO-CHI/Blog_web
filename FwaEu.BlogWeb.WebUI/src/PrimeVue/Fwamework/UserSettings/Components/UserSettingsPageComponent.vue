<template>
	<page-container type="form">
		<box v-if="currentUser && modelData.models" :title="$t('generalInformation')">
			<GeneralInformation v-if="userPartsFormItems && userPartsFormItems.length > 0" ref="generalInformation"
				:user="user" :userPartsFormItems="userPartsFormItems" :current-user="currentUser" />
			<div class="block" v-if="visibleModelsWhichAreComponents && visibleModelsWhichAreComponents.length > 0">
				<Accordion :activeIndex="0" :multiple="true">
					<AccordionTab v-for="(tabPanelModel, index ) in visibleModelsWhichAreComponents " :key="index"
						:collapsible="true" :deferRendering="false" :selectedIndex="-1" selection-mode="none"
						ref="accordion">
						<template #header>
							<div>
								{{ tabPanelModel.title }}
							</div>
						</template>
						<component :ref="tabPanelModel.ref" :is="tabPanelModel.handler.createComponent()"
							v-model="visibleModelsWhichAreComponents[index]" :fetched-data="tabPanelModel.fetchedData" />
					</AccordionTab>
				</Accordion>
			</div>
			<div class="form-buttons">
				<Button :label="$t('save')" @click="saveAsync" />
			</div>
		</box>
	</page-container>
</template>
<script>
import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
import Accordion from 'primevue/accordion';
import AccordionTab from 'primevue/accordiontab';
import Button from 'primevue/button';
import PageContainer from '@/Fwamework/PageContainer/Components/PageContainerComponent.vue';
import UserEditPageMixins from '@/Fwamework/Users/Services/user-edit-page-mixins';
import GeneralInformation from '@UILibrary/Fwamework/UserSettings/Components/GeneralInformationComponent.vue';
import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
import UserSettingsMixins from '@/Fwamework/Users/Services/user-settings-mixins';

export default {
	components: {
		Box,
		Accordion,
		AccordionTab,
		PageContainer,
		Button,
		GeneralInformation
	},
	mixins: [UserEditPageMixins, UserSettingsMixins],
	async created() {
		await loadMessagesAsync(this, import.meta.glob('@/Fwamework/UserSettings/Components/Content/user-settings-messages.*.json'));
	},
}
</script>

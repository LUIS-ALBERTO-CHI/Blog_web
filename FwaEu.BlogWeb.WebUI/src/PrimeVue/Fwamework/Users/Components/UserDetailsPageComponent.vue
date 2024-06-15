<template>
	<page-container type="form">
		<box v-if="user" :menu-items="menuItems" :timeline="timelineModel">
			<GeneralInformation v-if="userPartsFormItems && userPartsFormItems.length > 0" ref="generalInformation"
				:user="user" :userPartsFormItems="userPartsFormItems" :current-user="currentUser"
				@is-admin-changed="onIsAdminChangedAsync" />
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
import GeneralInformation from '@UILibrary/Fwamework/Users/Components/GeneralInformationComponent.vue';
import PageContainer from '@/Fwamework/PageContainer/Components/PageContainerComponent.vue';
import Button from 'primevue/button';
import UserEditPageMixins from '@/Fwamework/Users/Services/user-edit-page-mixins';
import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
import UserDetailsMixins from '@/Fwamework/Users/Services/user-details-mixins';
export default {
	components: {
		Box,
		Accordion,
		AccordionTab,
		GeneralInformation,
		PageContainer,
		Button,
	},
	mixins: [UserEditPageMixins, UserDetailsMixins],
	async created() {
		await loadMessagesAsync(this, import.meta.glob('@/Fwamework/Users/Components/Content/user-details-messages.*.json'));
	}
}
</script>

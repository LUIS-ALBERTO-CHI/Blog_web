<template>
	<page-container type="summary">
		<box title="{Change language}">
			<div>
				<dx-button @click="()=>setCurrentLanguage('fr')" text="FR" />
				<dx-button @click="()=>setCurrentLanguage('en')" text="EN" />
			</div>
			<div class="block">
				<strong>{{ $t('currentLanguage') }} {{ $i18n.locale.value }}</strong>
			</div>
			<p>
				{{ $t('localizationSamplePageMessage') }}
			</p>
		</box>
		<box :title="$t('loadedMessages')">
			<p>
				{{JSON.stringify($i18n.messages.value)}}
			</p>
		</box>
		<box :title="$t('culture.globalMessages')">
			<p>
				{{JSON.stringify(getGlobalMessages())}}
			</p>
		</box>
	</page-container>
</template>

<script>
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import DxButton from 'devextreme-vue/button';
	import { showLoadingPanel } from '@/Fwamework/LoadingPanel/Services/loading-panel-service';
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
	import { I18n } from '@/Fwamework/Culture/Services/localization-service';
	import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue";
	import { LocalStorage } from "@/Fwamework/Storage/Services/local-storage-store";

	export default {
		/*** In order to lazy load messages for a single file component, you need to :
		*** 1. delcare the LocalizationMixin
		*** 2. define the function i18n.messages.getMessagesAsync(locale) which retunrs an async import of json file with the component translations
		**/
		created: showLoadingPanel(async function ()
		{
			await this.slowActionAsync();

			await loadMessagesAsync(this, import.meta.glob('@/Samples/Culture/Components/Content/localization-sample-messages.*.json'));
		}),
		components: {
			Box,
			DxButton,
			PageContainer
		},
		methods: {
			slowActionAsync()
			{
				return new Promise((resolve) =>
				{
					setTimeout(resolve, 1000);
				});
			},
			setCurrentLanguage(language)
			{
				LocalStorage.setValue('LocalizationSampleForcedLanguage', language);
				location.reload();
			},
			getGlobalMessages()
			{
				return I18n.messages.value;
			}
		}
	}
</script>
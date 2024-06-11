<template>
	<page-container type="form">
		<box :title="getImportResultTitle()">
			<div>
				<div v-show="showUploadForm">
					<FileUpload 
								ref="fileUpload" 
								:chooseLabel="$t('chooseFileButton')"
								:accept="supportedFileExtensions" 
								:multiple="true"
								@select="onFileUploadValueChanged"
								:showUploadButton="false"
								:previewWidth="false"
								>
					<template #empty>
						<p>{{ $t('dragFileText') }}</p>
					</template>
					</FileUpload>
					<div class="form-buttons">
						<Button 
								:label="$t('uploadFileButton')" 
								@click="onFileUploadButtonClick" />
					</div>
				</div>
				<div v-if="importResult">
					<div>
						<Button 
								:label="$t('uploadOtherFiles')"
								@click="onUploadOtherFilesClick" />
					</div>
					<div class="block">
						<ProcessResult :results="importResult.results"></ProcessResult>
					</div>
				</div>
			</div>
		</box>
	</page-container>
</template>
<script>
import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue";
import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
import ProcessResult from '@UILibrary/Fwamework/ProcessResults/Components/ProcessResultComponent.vue';
import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";
import FileUpload from 'primevue/fileupload'; 
import Button from 'primevue/button';
import { Configuration } from '@/Fwamework/Core/Services/configuration-service';
import DataImportService from '@/Modules/DataImport/Services/data-import-service'

export default {
	components: {
		PageContainer,
		Box,
		FileUpload,
		ProcessResult,
		Button,
	},
	data() {
		return {
			importResult: null,
			showUploadForm: true,
			files: []
		};
	},
	async created() {
		await loadMessagesAsync(this, import.meta.glob('@/Modules/DataImport/Components/Content/import-messages.*.json'));
	},
	methods: {
		getImportResultTitle() {
			return this.importResult ? this.$t('importResult') : ""
		},
		onFileUploadButtonClick: showLoadingPanel(async function () {
			let results = null;

			await DataImportService.importDataAsync(this.files).then(function (data) {
				results = data;
			})

			this.importResult = results;
			this.showUploadForm = results === null;
			this.files = [];

		}),
		onFileUploadValueChanged: showLoadingPanel(async function (e) {
			this.files = e.files;
		}),
		onUploadOtherFilesClick() {
			this.showUploadForm = true;
			this.importResult = null;
			this.$refs.fileUpload.clear();
		},
	},
	computed: {
		supportedFileExtensions() {
			return Configuration.dataImport.supportedFileExtensions.join(',');
		}
	}
}
</script>


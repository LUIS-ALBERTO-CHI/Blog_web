<template>
	<page-container type="form">
		<box :title="getImportResultTitle()">
			<div>
				<div v-show="showUploadForm">
					<dx-file-uploader ref="fileUpload"
									  :accept="supportedFileExtensions"
									  :multiple="true"
									  upload-mode="useForm"
									  :label-text="$t('dragFileText')"
									  :select-button-text="$t('chooseFileButton')"
									  @value-changed="onFileUploadValueChanged" />
					<div class="form-buttons">
						<dx-button :text="$t('uploadFileButton')"
								   type="success"
								   @click="onFileUploadButtonClick" />
					</div>
				</div>
				<div v-if="importResult">
					<div>
						<dx-button :text="$t('uploadOtherFiles')"
								   type="default"
								   @click="onUploadOtherFilesClick" />
					</div>
					<div class="block">
						<process-result :results="importResult.results">
						</process-result>
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
	import { DxFileUploader } from 'devextreme-vue/file-uploader';
	import { DxButton } from 'devextreme-vue/button';
	import { Configuration } from '@/Fwamework/Core/Services/configuration-service';
	import DataImportService from '@/Modules/DataImport/Services/data-import-service'

	export default {
		components: {
			PageContainer,
			Box,
			DxFileUploader,
			ProcessResult,
			DxButton
		},
		data() {
			return {
				uploadMode: 'useButtons',
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
			onFileUploadValueChanged(e) {
				this.files = e.value;
			},
			onUploadOtherFilesClick() {
				this.showUploadForm = true;
				this.importResult = null;
				this.$refs.fileUpload.instance.reset();
			},
		},
		computed: {
			supportedFileExtensions() {
				return Configuration.dataImport.supportedFileExtensions.join(',');
			}
		}
	}

</script>


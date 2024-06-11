<template>
	<box :title="$t('title')">
		<dx-accordion>
			<dx-item :title="$t('importFromAppData')">
				<template #default>
					<div>
						<dx-tree-list v-if="fileItems"
									  ref="importFilesTreeList"
									  :data-source="fileItems"
									  :auto-expand-all="true"
									  items-expr="items"
									  data-structure="tree"
									  :height="600">
							<dx-filter-row :visible="true" />
							<dx-selection :recursive="true"
										  :allow-select-all="true"
										  mode="multiple" />
							<dx-column data-field="name"
									   cell-template="fileNameTemplate" />
							<template #fileNameTemplate="{data}">
								<div>
									<i :class="`dx-icon-${getIconName(data.data)}`" />
									{{data.data.name}}
								</div>
							</template>
							<dx-column :width="200"
									   data-field="size" />
						</dx-tree-list>

						<dx-button @click="importSelectedFiles" :text="$t('importSelectedFile')"></dx-button>
						<dx-button @click="downloadSelectedFiles" :text="$t('downloadSelectedFile')"></dx-button>
					</div>
				</template>

			</dx-item>

			<dx-item :title="$t('titleImportByUpload')">
				<template #default>
					<box class="block">
						<dx-file-uploader ref="fileUpload"
										  :accept="supportedFileExtensions"
										  :multiple="true"
										  upload-mode="useForm"
										  @value-changed="onFileUploadValueChanged" />
						<dx-button :text="$t('importSelectedFile')"
								   @click="onFileUploadButtonClick" />
					</box>
				</template>
			</dx-item>
		</dx-accordion>
		<process-result v-if="taskResult" :results="taskResult.results">
		</process-result>
	</box>
</template>
<script>
	import { DxButton } from 'devextreme-vue/button';
	import DxFileUploader from 'devextreme-vue/file-uploader';
	import ProcessResult from '@UILibrary/Fwamework/ProcessResults/Components/ProcessResultComponent.vue';
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
	import SetupService from '@/Fwamework/Setup/Services/setup-service';
	import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";
	import FilesManagement from "@UILibrary/Extensions/Services/files-management-service";
	import DataService from "@/Fwamework/Data/Services/data-service";
	import HttpService from '@/Fwamework/Core/Services/http-service';
	import { DxTreeList, DxSelection, DxColumn, DxFilterRow } from 'devextreme-vue/tree-list';
	import Box from '@/Fwamework/Box/Components/BoxComponent.vue';
	import DxAccordion, { DxItem } from "devextreme-vue/accordion";
	import { Configuration } from '@/Fwamework/Core/Services/configuration-service';

	export default {
		components: {
			ProcessResult,
			DxFileUploader,
			DxButton,
			DxTreeList,
			DxSelection,
			DxColumn,
			DxFilterRow,
			Box,
			DxAccordion,
			DxItem
		},
		props: {
			setupTask: Object
		},

		data() {
			return {
				taskResult: null,
				fileItems: null,
				files: [],
				uploadedFiles: []
			};
		},

		created: showLoadingPanel(async function () {
			let importFilesTaskResult = await SetupService.executeSetupTaskAsync("ImportFileList", null);
			if (importFilesTaskResult.results.contexts.length !== 0) {
				this.taskResult = importFilesTaskResult;
			}
			this.files = importFilesTaskResult.data.files;
			this.fileItems = FilesManagement.createFileManagerFiles(this.files);

			await loadMessagesAsync(this, import.meta.glob('@/Fwamework/SetupImport/Components/Content/import-files-messages.*.json'));
		}),

		methods: {
			importSelectedFiles: showLoadingPanel(async function () {
				let fileIds = this.getSelectedFilesIds();
				this.taskResult = await SetupService.executeSetupTaskAsync("ImportFilesById", { fileIds });
			}),

			downloadSelectedFiles: showLoadingPanel(async function () {
				let fileIds = this.getSelectedFilesIds();
				let downloadedFiles = await SetupService.executeSetupTaskAsync("DownloadFiles", { fileIds });

				let $this = this;
				downloadedFiles.data.fileContents.forEach(fc => {
					let file = $this.files.find(f => f.id === fc.fileId);
					let blobFile = DataService.convertBase64ToBlob(fc.base64Content, file.contentType);
					HttpService.saveBlobFile(blobFile, false, DataService.getFileName(file.filePath));
				});
			}),

			getSelectedFilesIds() {
				let selectedItems = this.$refs.importFilesTreeList.instance.getSelectedRowsData("all");
				return selectedItems.filter(f => !f.isDirectory).map(f => f.id);
			},

			onFileUploadValueChanged: showLoadingPanel(async function (e) {
				const convertFilesPromises = e.value.map(file => DataService.convertFileToBase64Async(file)
					.then(base64File => {
						return { fileName: file.name, content: base64File };
					}));
				this.uploadedFiles = await Promise.all(convertFilesPromises);
			}),

			onFileUploadButtonClick: showLoadingPanel(async function () {
				this.taskResult = await SetupService.executeSetupTaskAsync('UploadBase64File', { files: this.uploadedFiles });
				this.$refs.fileUpload.instance.reset();
			}),

			getIconName(item) {
				if (item.isDirectory)
					return 'inactivefolder';
				else {
					let fileExtension = item.name.split('.').pop();
					switch (fileExtension) {
						case "xlsx":
							return "xlsxfile";
						case "xls":
							return "xlsfile";
						default:
							return "file";
					}
				}
			}
		},
		computed: {
			supportedFileExtensions() {
				return Configuration.dataImport.supportedFileExtensions.join(',');
			}
		}
	}
</script>
import { defineAsyncComponent } from "vue";

export default {
	taskName: "ImportFileList",
	createComponent: () => defineAsyncComponent(() => import("@UILibrary/Fwamework/SetupImport/Components/ImportFilesComponent.vue"))
}

import { defineAsyncComponent } from "vue";

export default {
	taskName: "BackgroundTasks",
	createComponent: () => defineAsyncComponent(() => import("@UILibrary/Modules/BackgroundTasks/Components/BackgroundTasksSetupTaskResultComponent.vue"))
}
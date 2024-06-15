import { defineAsyncComponent } from "vue";

export default {
	taskName: "DatabaseTask",
	createComponent: () => defineAsyncComponent(() => import("@/Fwamework/SetupData/Components/DatabaseComponent.vue"))
}

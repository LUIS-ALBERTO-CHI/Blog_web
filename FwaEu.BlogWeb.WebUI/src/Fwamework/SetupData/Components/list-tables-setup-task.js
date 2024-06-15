import { defineAsyncComponent } from "vue";

export default {
	taskName: "ListTables",
	createComponent: () => defineAsyncComponent(() => import("@UILibrary/Fwamework/SetupData/Components/ListTablesComponent.vue"))
}

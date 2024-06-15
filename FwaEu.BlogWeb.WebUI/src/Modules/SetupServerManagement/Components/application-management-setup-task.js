import { defineAsyncComponent } from "vue";

export default {
	taskName: "ServerApplicationManagement",
	createComponent: () => defineAsyncComponent(() => import("@UILibrary/Modules/SetupServerManagement/Components/ApplicationManagementComponent.vue"))
}

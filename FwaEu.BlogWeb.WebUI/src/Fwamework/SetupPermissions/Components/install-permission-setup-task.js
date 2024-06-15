import { defineAsyncComponent } from "vue";

export default {
	taskName: "CreatePermissions",
	createComponent: () => defineAsyncComponent(() => import("@/Fwamework/SetupPermissions/Components/InstallPermissionComponent.vue"))
}

import { defineAsyncComponent } from "vue";

export default {
	taskName: "SmtpOptions",
	createComponent: () => defineAsyncComponent(() => import("@UILibrary/Modules/SetupMail/Components/SmtpSetupTaskResultComponent.vue"))
}
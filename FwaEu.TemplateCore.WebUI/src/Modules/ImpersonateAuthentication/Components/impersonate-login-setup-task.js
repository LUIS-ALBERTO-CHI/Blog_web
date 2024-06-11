import { defineAsyncComponent } from "vue";

export default {
	taskName: "Impersonate",
	createComponent: () => defineAsyncComponent(() => import("@UILibrary/Modules/ImpersonateAuthentication/Components/ImpersonateLoginComponent.vue"))
}

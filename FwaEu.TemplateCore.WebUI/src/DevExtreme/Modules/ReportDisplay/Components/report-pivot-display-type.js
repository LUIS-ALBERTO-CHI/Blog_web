import { I18n } from "@/Fwamework/Culture/Services/localization-service"
import { defineAsyncComponent } from "vue";

export default {
	type: "pivot",
	createComponent: () => defineAsyncComponent(() => import("@UILibrary/Modules/ReportDisplay/Components/ReportPivotDisplayComponent.vue")),
	getDescriptionKey() {
		return {
			label: I18n.t("pivotLabel"),
			icon: "fad fa-file-excel"
		};
	}
}


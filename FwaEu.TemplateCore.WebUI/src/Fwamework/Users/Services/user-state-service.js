import { I18n } from "@/Fwamework/Culture/Services/localization-service";

export default {
	getAll() {
		return [{ text: I18n.t('active'), value: "Active"}, { text: I18n.t('disabled'), value: "Disabled"}];
	}
}
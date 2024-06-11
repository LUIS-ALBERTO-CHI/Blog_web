import { I18n } from "@/Fwamework/Culture/Services/localization-service";

export default { 
    notificationType: 'NewFarmer',
    async getMessageAsync (argument) {
        return I18n.t('persistentNotificationNewFarmer', [argument.pseudonym]);
    }
}
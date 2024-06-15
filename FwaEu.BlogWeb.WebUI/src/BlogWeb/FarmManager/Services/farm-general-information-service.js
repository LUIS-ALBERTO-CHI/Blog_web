import HttpService from "@/Fwamework/Core/Services/http-service";
import NotificationService from "@/Fwamework/Notifications/Services/notification-service";
import { I18n } from "@/Fwamework/Culture/Services/localization-service";

export default {
	async updateAsync(id, farmSaveGeneralInformationModel) {
		let result = await HttpService.put(`Farms/${id}/GeneralInformation`, farmSaveGeneralInformationModel).catch(reason => {
			checkForNotFoundError(reason, id);
			checkForConflictError(reason, farmSaveGeneralInformationModel.name);
			throw reason;
		});
		return result;
	},
	async saveAsync(farmSaveGeneralInformationModel) {
		let result = await HttpService.post(`Farms`, farmSaveGeneralInformationModel).catch(reason => {
			checkForConflictError(reason, farmSaveGeneralInformationModel.name);
			throw reason;
		});
		return result.data;
	},
	async getAsync(id) {
		let result = await HttpService.get(`Farms/${id}/GeneralInformation`).catch(reason => {
			checkForNotFoundError(reason, id);
			throw reason;
		});
		let farm = result.data;
		farm.createdOn = new Date(farm.createdOn);
		farm.updatedOn = new Date(farm.updatedOn);
		return farm;
	},
	async deleteAsync(id) {
		let result = await HttpService.delete(`Farms/${id}`).catch(reason => {
			checkForNotFoundError(reason, id);
			throw reason;
		});
		return result;
	}
}

function checkForConflictError(reason, farmName) {
	if (reason.response?.data?.type === "UniqueDbConstraint" && reason.response?.status === 409) {
		reason.isHandled = true;
		NotificationService.showError(I18n.t('errorFarmAlreadyExists', { farmName: farmName }));
	}
}

function checkForNotFoundError(reason, id) {
	if (reason.response?.status === 404) {
		reason.isHandled = true;
		NotificationService.showError(I18n.t('errorFarmNotFound', { farmId: id }));
	}
}

import HttpService from "@/Fwamework/Core/Services/http-service";

export default {
	async getAllByFarmIdAsync(farmId) {
		let result = await HttpService.get(`Farms/${farmId}/AnimalsCount`);
		result.data.forEach(x => {
			x.updatedOn = new Date(x.updatedOn);
		});

		return result.data;
	},
	async saveOrDeleteByFarmIdAsync(farmId, animalCountBySpeciesModel) {
		let result = await HttpService.put(`Farms/${farmId}/AnimalsCount`, animalCountBySpeciesModel);
		return result;
	}
}
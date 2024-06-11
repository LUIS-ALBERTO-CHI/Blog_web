import HttpService from "@/Fwamework/Core/Services/http-service";

export default {

	async getAllAsync(onlyFarmsWithoutAnimals) {
		const result = await HttpService.get(`farms?onlyFarmsWithoutAnimals=${onlyFarmsWithoutAnimals}`);
		result.data.forEach(farm => {
			farm.updatedOn = new Date(farm.updatedOn);
			farm.openingDate = new Date(farm.openingDate);

			if (farm.closingDate) {

				farm.closingDate = new Date(farm.closingDate);
			}
		});
		return result.data;
	}
}
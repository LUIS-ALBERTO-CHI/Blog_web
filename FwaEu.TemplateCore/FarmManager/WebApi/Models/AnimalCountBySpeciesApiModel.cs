using System;

namespace FwaEu.TemplateCore.FarmManager.WebApi.Models
{
	public class AnimalCountBySpeciesApiModel
	{
		public AnimalCountBySpeciesApiModel(
			int quantity, int animalSpeciesId,
			DateTime updatedOn, int? updatedById)
		{
			Quantity = quantity;
			AnimalSpeciesId = animalSpeciesId;
			UpdatedOn = updatedOn;
			UpdatedById = updatedById;
		}

		public int Quantity { get; }
		public int AnimalSpeciesId { get; }
		public DateTime UpdatedOn { get; }
		public int? UpdatedById { get; }
	}
}

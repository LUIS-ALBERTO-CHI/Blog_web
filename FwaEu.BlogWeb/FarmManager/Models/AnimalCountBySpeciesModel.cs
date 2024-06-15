using System;

namespace FwaEu.BlogWeb.FarmManager.Models
{
	public class AnimalCountBySpeciesModel
	{
		public AnimalCountBySpeciesModel(
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

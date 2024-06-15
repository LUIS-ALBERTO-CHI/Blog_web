using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.FarmManager.Models
{
	public class AnimalCountSaveModel
	{
		public AnimalCountSaveModel(int quantity, int animalSpeciesId)
		{
			Quantity = quantity;
			AnimalSpeciesId = animalSpeciesId;
		}

		public int Quantity { get; }
		public int AnimalSpeciesId { get; }
	}
}

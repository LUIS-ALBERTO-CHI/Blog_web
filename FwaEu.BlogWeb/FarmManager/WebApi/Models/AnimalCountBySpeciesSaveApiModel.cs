using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.FarmManager.WebApi.Models
{
	public class AnimalCountBySpeciesSaveApiModel
	{
		[Required]
		public int? Quantity { get; set; }

		[Required]
		public int? AnimalSpeciesId { get; set; }
	}
}

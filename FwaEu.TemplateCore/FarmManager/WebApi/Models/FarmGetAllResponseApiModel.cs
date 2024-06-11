using System;
using FwaEu.TemplateCore.FarmManager.Entities;

namespace FwaEu.TemplateCore.FarmManager.WebApi.Models
{
	public class FarmGetAllResponseApiModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int? PostalCodeId { get; set; }
		public FarmCategorySize? CategorySize { get; set; }
		public int MainActivityId { get; set; }
		public decimal? SellingPriceInEurosWithoutTaxes { get; set; }
		public bool RecruitEmployees { get; set; }
		public DateTime OpeningDate { get; set; }
		public DateTime? ClosingDate { get; set; }
		public int AnimalCount { get; set; }
		public DateTime? UpdatedOn { get; set; }
		public int? UpdatedById { get; set; }
		public int? RegionId { get; set; }
	}
}

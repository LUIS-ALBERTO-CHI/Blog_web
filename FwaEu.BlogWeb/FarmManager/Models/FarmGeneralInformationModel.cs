using System;
using FwaEu.BlogWeb.FarmManager.Entities;

namespace FwaEu.BlogWeb.FarmManager.Models
{
	public class FarmGeneralInformationModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int PostalCodeId { get; set; }
		public FarmCategorySize? CategorySize { get; set; }
		public int MainActivityId { get; set; }
		public decimal? SellingPriceInEurosWithoutTaxes { get; set; }
		public bool RecruitEmployees { get; set; }
		public DateTime OpeningDate { get; set; }
		public DateTime? ClosingDate { get; set; }
		public string Comments { get; set; }
		public DateTime CreatedOn { get; set; }
		public int? CreatedById { get; set; }
		public DateTime UpdatedOn { get; set; }
		public int? UpdatedById { get; set; }
	}
}

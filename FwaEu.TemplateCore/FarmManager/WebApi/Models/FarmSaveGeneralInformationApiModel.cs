using System;
using System.ComponentModel.DataAnnotations;
using FwaEu.TemplateCore.FarmManager.Entities;


namespace FwaEu.TemplateCore.FarmManager.WebApi.Models
{
	public class FarmSaveGeneralInformationApiModel
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public int? PostalCodeId { get; set; }

		[Required]
		public FarmCategorySize? CategorySize { get; set; }

		[Required]
		public int? MainActivityId { get; set; }

		public decimal? SellingPriceInEurosWithoutTaxes { get; set; }

		[Required]
		public bool? RecruitEmployees { get; set; }

		[Required]
		public DateTime? OpeningDate { get; set; }

		public DateTime? ClosingDate { get; set; }

		public string Comments { get; set; }
	}
}

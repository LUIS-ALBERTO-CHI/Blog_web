using System;
using System.ComponentModel.DataAnnotations;

namespace FwaEu.Modules.SearchEngine.WebApi
{
	public class SearchParametersApiModel
	{
		[Required]
		public string Key { get; set; }

		[Required]
		public int? Skip { get; set; }

		[Required]
		public int? Take { get; set; }
	}
}

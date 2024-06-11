using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.MasterData.WebApi
{
	public class GetChangeInfosParametersModel
	{
		[Required]
		public string MasterDataKey { get; set; }
	}

	public class GetModelsParametersModel
	{
		[Required]
		public string MasterDataKey { get; set; }

		public PaginationModel Pagination { get; set; }
		public string Search { get; set; }
		public OrderByModel[] OrderBy { get; set; }
	}

	public class OrderByModel
	{
		[Required]
		public string PropertyName { get; set; }

		[Required]
		public bool? Ascending { get; set; }
	}

	public class PaginationModel
	{
		[Required]
		public int? Skip { get; set; }

		[Required]
		public int? Take { get; set; }
	}

	public class GetModelsByIdsParametersModel
	{
		[Required]
		public string MasterDataKey { get; set; }

		[Required]
		public JArray Ids { get; set; }
	}
}

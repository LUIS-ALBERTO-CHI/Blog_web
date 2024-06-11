using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FwaEu.Modules.GenericAdmin.WebApi
{
	public class SortParameterModel
	{
		[Required]
		public string ColumnName { get; set; }
		public bool? Ascending { get; set; }
	}

	public class SortParametersModel
	{
		public List<SortParameterModel> Parameters { get; set; }
	}
}

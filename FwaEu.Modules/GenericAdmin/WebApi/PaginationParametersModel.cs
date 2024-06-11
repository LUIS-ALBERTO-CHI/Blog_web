namespace FwaEu.Modules.GenericAdmin.WebApi
{
	public class PaginationParametersModel
	{
		public bool? RequireTotalCount { get; set; }
		public int? Skip { get; set; }
		public int? Take { get; set; }
	}
}

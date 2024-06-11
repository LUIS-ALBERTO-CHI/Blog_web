namespace FwaEu.Modules.GenericAdmin.WebApi
{
	public class GetModelsParametersModel
	{
		public PaginationParametersModel Pagination {  get; set; }
		public SortParametersModel Sort { get; set; }
		public FilterParametersModel Filter { get; set; }
	}
}

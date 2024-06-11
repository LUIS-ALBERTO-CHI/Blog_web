namespace FwaEu.Modules.GenericAdmin
{
	public class GenericAdminPaginationParameters
	{
		public bool RequireTotalCount { get; }
		public int? Skip { get; }
		public int? Take { get; }

		public GenericAdminPaginationParameters(bool requireTotalCount, int? skip, int? take)
		{
			RequireTotalCount = requireTotalCount;
			Skip = skip;
			Take = take;
		}
	}
}

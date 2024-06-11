using System.Globalization;

namespace FwaEu.Modules.GenericAdmin
{
	public class GenericAdminGetModelsParameters
	{
		public CultureInfo UserCulture { get; }
		public GenericAdminPaginationParameters Pagination { get; }
		public GenericAdminSortParameters Sort { get; }
		public GenericAdminFilterParameters Filter { get; }

		public GenericAdminGetModelsParameters(
			CultureInfo culture,
			GenericAdminPaginationParameters pagination,
			GenericAdminSortParameters sort,
            GenericAdminFilterParameters filter)
		{
			UserCulture = culture;
			Pagination = pagination;
			Sort = sort;
            Filter = filter;
		}
	}
}

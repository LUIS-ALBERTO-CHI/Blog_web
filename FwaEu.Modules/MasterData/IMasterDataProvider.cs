using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.MasterData
{
	public class MasterDataProviderParametersBase
	{
	}

	public class MasterDataProviderGetChangesParameters : MasterDataProviderParametersBase
	{
	}

	public class MasterDataProviderGetModelsParameters : MasterDataProviderParametersBase
	{
		public MasterDataProviderGetModelsParameters(MasterDataPaginationParameters pagination,
			string search, OrderByParameter[] orderBy, CultureInfo culture)
		{
			this.Pagination = pagination;
			this.Search = search;
			this.OrderBy = orderBy;
			this.Culture = culture ?? throw new ArgumentNullException(nameof(culture));
		}

		public MasterDataPaginationParameters Pagination { get; }
		public string Search { get; }
		public OrderByParameter[] OrderBy { get; }
		public CultureInfo Culture { get; }
	}

	public class MasterDataProviderGetModelsByIdsParameters : MasterDataProviderParametersBase
	{
		public MasterDataProviderGetModelsByIdsParameters(object[] ids, CultureInfo culture)
		{
			this.Ids = ids ?? throw new ArgumentNullException(nameof(ids));
			this.Culture = culture ?? throw new ArgumentNullException(nameof(culture));
		}

		public object[] Ids { get; set; }
		public CultureInfo Culture { get; }
	}

	public class OrderByParameter
	{
		public OrderByParameter(string propertyName, bool ascending)
		{
			this.PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
			this.Ascending = ascending;
		}

		public string PropertyName { get; }
		public bool Ascending { get; }
	}

	public class MasterDataChangesInfo
	{
		public MasterDataChangesInfo(DateTime? maximumUpdatedOn, int count)
		{
			this.MaximumUpdatedOn = maximumUpdatedOn;
			this.Count = count;
		}

		public DateTime? MaximumUpdatedOn { get; }
		public int Count { get; }
	}

	public interface IMasterDataProvider
	{
		Task<MasterDataChangesInfo> GetChangesInfoAsync(MasterDataProviderGetChangesParameters parameters);
		Task<IEnumerable> GetModelsAsync(MasterDataProviderGetModelsParameters parameters);
		Task<IEnumerable> GetModelsByIdsAsync(MasterDataProviderGetModelsByIdsParameters parameters);
		Type IdType { get; }
	}
}

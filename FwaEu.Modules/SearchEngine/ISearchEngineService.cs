using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.SearchEngine
{
	public interface ISearchEngineService
	{
		Task<SearchResultModel[]> SearchAsync(string search, SearchParameters[] parameters);
	}

	public class SearchParameters
	{
		public SearchParameters(string providerKey, SearchPagination pagination)
		{
			this.ProviderKey = providerKey
				?? throw new ArgumentNullException(nameof(providerKey));

			this.Pagination = pagination
				?? throw new ArgumentNullException(nameof(pagination));
		}

		public string ProviderKey { get; }
		public SearchPagination Pagination { get; }
	}

	public class SearchPagination
	{
		public SearchPagination(int skip, int take)
		{
			this.Skip = skip;
			this.Take = take;
		}

		public int Skip { get; }
		public int Take { get; }
	}

	public class SearchResultModel
	{
		public SearchResultModel(string key, IEnumerable<object> results)
		{
			this.Key = key ?? throw new ArgumentNullException(nameof(key));
			this.Results = results ?? throw new ArgumentNullException(nameof(results));
		}

		public string Key { get; }
		public IEnumerable<object> Results { get; }
	}
}

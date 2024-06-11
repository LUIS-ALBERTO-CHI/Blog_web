using System.Collections.Generic;
using System.Threading.Tasks;

namespace FwaEu.Modules.SearchEngine
{

	public interface ISearchEngineResultProvider
	{
		Task<IEnumerable<object>> SearchAsync(string search, SearchPagination pagination);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.SearchEngine.WebApi
{
	public class SearchResultApiModel
	{
		public SearchResultApiModel(string key, object[] results)
		{
			this.Key = key ?? throw new ArgumentNullException(nameof(key));
			this.Results = results ?? throw new ArgumentNullException(nameof(results));
		}

		public string Key { get; }
		public object[] Results { get; }
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Reports
{
	public class FiltersParametersProvider : IParametersProvider
	{
		private Dictionary<string, object> _filters;

		public void SetFilters(Dictionary<string, object> filters)
		{
			this._filters = filters ?? throw new ArgumentNullException(nameof(filters));
		}
		public Task<Dictionary<string, object>> LoadAsync()
		{
			return Task.FromResult(this._filters ?? new Dictionary<string, object>());
		}
	}
}

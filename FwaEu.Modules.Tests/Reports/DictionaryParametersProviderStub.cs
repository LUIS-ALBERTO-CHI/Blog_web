using FwaEu.Modules.Reports;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.Reports
{
	public class DictionaryParametersProviderStub : IParametersProvider
	{
		private readonly Dictionary<string, object> _data;

		public DictionaryParametersProviderStub(Dictionary<string, object> data)
		{
			this._data = data ?? throw new ArgumentNullException(nameof(data));
		}

		public Task<Dictionary<string, object>> LoadAsync()
		{
			return Task.FromResult(this._data);
		}
	}
}

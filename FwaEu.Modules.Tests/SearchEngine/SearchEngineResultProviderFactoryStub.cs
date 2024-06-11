using FwaEu.Modules.SearchEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.SearchEngine
{
	public class SearchEngineResultProviderFactoryStub : ISearchEngineResultProviderFactory
	{
		private readonly Func<ISearchEngineResultProvider> _create;

		public SearchEngineResultProviderFactoryStub(Func<ISearchEngineResultProvider> create, string key)
		{
			this._create = create ?? throw new ArgumentNullException(nameof(create));
			this.Key = key ?? throw new ArgumentNullException(nameof(key));
		}

		public string Key { get; }

		public ISearchEngineResultProvider Create()
		{
			return this._create();
		}
	}
}

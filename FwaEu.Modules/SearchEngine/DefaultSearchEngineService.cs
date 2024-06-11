using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.SearchEngine
{
	public class DefaultSearchEngineService : ISearchEngineService
	{
		private readonly IEnumerable<ISearchEngineResultProviderFactory> _factories;

		public DefaultSearchEngineService(IEnumerable<ISearchEngineResultProviderFactory> factories)
		{
			this._factories = factories
				?? throw new ArgumentNullException(nameof(factories));
		}

		public async Task<SearchResultModel[]> SearchAsync(string search, SearchParameters[] parameters)
		{
			var existingFactories = parameters
				.Join(this._factories, p => p.ProviderKey, f => f.Key, (p, f) => new
				{
					Factory = f,
					Pagination = p.Pagination,
				})
				.ToArray();

			if (existingFactories.Length < parameters.Length)
			{
				var notFound = parameters
					.Select(p => p.ProviderKey)
					.Except(existingFactories.Select(ef => ef.Factory.Key))
					.ToArray();

				throw new ProvidersNotFoundException(notFound);
			}

			//NOTE: Do not run in parallel because NHibernate cannot handle it https://github.com/nhibernate/nhibernate-core/issues/1642
			var searchResults = new SearchResultModel[existingFactories.Length];
			for (int i = 0; i < existingFactories.Length; i++)
			{
				var existingFactory = existingFactories[i];
				var result = await existingFactory.Factory.Create().SearchAsync(search, existingFactory.Pagination);
				searchResults[i] = new SearchResultModel(existingFactory.Factory.Key, result);
			}

			return searchResults;
		}
	}
}

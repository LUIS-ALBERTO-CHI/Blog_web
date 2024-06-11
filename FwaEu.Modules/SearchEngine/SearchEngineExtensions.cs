using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.SearchEngine
{
	public static class SearchEngineExtensions
	{
		public static IServiceCollection AddFwameworkModuleSearchEngine(this IServiceCollection services)
		{
			services.AddTransient<ISearchEngineService, DefaultSearchEngineService>();
			return services;
		}

		public static IServiceCollection AddSearchEngineResultProvider<TProvider>(this IServiceCollection services, string key)
			where TProvider : class, ISearchEngineResultProvider
		{
			services.AddTransient<ISearchEngineResultProviderFactory>(sp =>
				new InjectedSearchEngineResultProviderFactory<TProvider>(sp, key));

			services.AddTransient<TProvider>();

			return services;
		}

		public static IQueryable<T> SkipTake<T>(this IQueryable<T> query, SearchPagination pagination)
		{
			return query
				.Skip(pagination.Skip)
				.Take(pagination.Take);
		}
	}
}

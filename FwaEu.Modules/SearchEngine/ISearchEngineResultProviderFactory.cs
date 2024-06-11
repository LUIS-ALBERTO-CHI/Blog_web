using Microsoft.Extensions.DependencyInjection;
using System;

namespace FwaEu.Modules.SearchEngine
{
	public interface ISearchEngineResultProviderFactory
	{
		string Key { get; }
		ISearchEngineResultProvider Create();
	}

	public class InjectedSearchEngineResultProviderFactory<TProvider> : ISearchEngineResultProviderFactory
		where TProvider : class, ISearchEngineResultProvider
	{
		public InjectedSearchEngineResultProviderFactory(IServiceProvider serviceProvider, string key)
		{
			this.ServiceProvider = serviceProvider
				?? throw new ArgumentNullException(nameof(serviceProvider));

			this.Key = key ?? throw new ArgumentNullException(nameof(key));
		}

		public IServiceProvider ServiceProvider { get; }
		public string Key { get; }

		public ISearchEngineResultProvider Create()
		{
			return this.ServiceProvider.GetRequiredService<TProvider>();
		}
	}
}

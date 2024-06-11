using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.MasterData
{
	public interface IMasterDataProviderFactory
	{
		string Key { get; }
		IMasterDataProvider Create(IServiceProvider serviceProvider);
	}

	public class MasterDataFactoryCache
	{
		public MasterDataFactoryCache(IEnumerable<IMasterDataProviderFactory> factories)
		{
			if (factories == null)
			{
				throw new ArgumentNullException(nameof(factories));
			}

			this._factoriesByKey = new Lazy<IDictionary<string, IMasterDataProviderFactory>>(
				() => factories.ToDictionary(f => f.Key));
		}

		private Lazy<IDictionary<string, IMasterDataProviderFactory>> _factoriesByKey;

		public IMasterDataProviderFactory GetFactory(string key)
		{
			var cache = this._factoriesByKey.Value;
			return cache.ContainsKey(key) ? cache[key] : null;
		}
	}
}

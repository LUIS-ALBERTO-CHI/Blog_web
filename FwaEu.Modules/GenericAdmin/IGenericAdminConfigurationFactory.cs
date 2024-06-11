using System.Collections.Generic;
using System;
using System.Linq;

namespace FwaEu.Modules.GenericAdmin
{
	public interface IGenericAdminConfigurationFactory
	{
		string Key { get; }
		IGenericAdminModelConfiguration Create(IServiceProvider serviceProvider);
	}

	public class GenericAdminConfigurationFactoryCache
	{
		public GenericAdminConfigurationFactoryCache(IEnumerable<IGenericAdminConfigurationFactory> factories)
		{
			if (factories == null)
			{
				throw new ArgumentNullException(nameof(factories));
			}

			this._factoriesByKey = new Lazy<IDictionary<string, IGenericAdminConfigurationFactory>>(
				() => factories.ToDictionary(f => f.Key));
		}

		private Lazy<IDictionary<string, IGenericAdminConfigurationFactory>> _factoriesByKey;

		public IGenericAdminConfigurationFactory GetFactory(string key)
		{
			var cache = this._factoriesByKey.Value;
			return cache.ContainsKey(key) ? cache[key] : null;
		}
	}
}

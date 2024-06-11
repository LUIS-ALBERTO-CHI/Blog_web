using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Users.UserPerimeter
{
	public interface IUserPerimeterProviderFactory
	{
		string Key { get; }
		IUserPerimeterProvider Create(IServiceProvider serviceProvider);
	}

	public class UserPerimeterProviderFactoryCache
	{
		public UserPerimeterProviderFactoryCache(IEnumerable<IUserPerimeterProviderFactory> factories)
		{
			if (factories == null)
			{
				throw new ArgumentNullException(nameof(factories));
			}

			this._factoriesByKey = new Lazy<IDictionary<string, IUserPerimeterProviderFactory>>(
				() => factories.ToDictionary(f => f.Key));
		}

		private Lazy<IDictionary<string, IUserPerimeterProviderFactory>> _factoriesByKey;

		public IUserPerimeterProviderFactory GetFactory(string key)
		{
			var cache = this._factoriesByKey.Value;
			return cache.ContainsKey(key) ? cache[key] : null;
		}

		public IEnumerable<IUserPerimeterProviderFactory> GetAll()
		{
			return this._factoriesByKey.Value.Values;
		}
	}
}

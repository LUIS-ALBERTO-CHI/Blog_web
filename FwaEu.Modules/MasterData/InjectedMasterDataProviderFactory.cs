using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.MasterData
{
	public class InjectedMasterDataProviderFactory<TProvider> : IMasterDataProviderFactory
			where TProvider : class, IMasterDataProvider
	{
		public InjectedMasterDataProviderFactory(string key)
		{
			this.Key = key ?? throw new ArgumentNullException(nameof(key));
		}

		public string Key { get; }

		public IMasterDataProvider Create(IServiceProvider serviceProvider)
		{
			return serviceProvider.GetRequiredService<TProvider>();
		}
	}
}

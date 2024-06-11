using FwaEu.Fwamework.Data.Database;
using FwaEu.Modules.MasterData;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.SimpleMasterData.MasterData
{
	public class SimpleMasterDataProviderFactory<TEntity, TProvider> : IMasterDataProviderFactory
		where TEntity : SimpleMasterDataEntityBase
		where TProvider : IMasterDataProvider
	{
		public SimpleMasterDataProviderFactory(IEntityKeyResolver<TEntity> keyResolver)
		{
			_ = keyResolver ?? throw new ArgumentNullException(nameof(keyResolver));
			this._keyLazy = new Lazy<string>(keyResolver.ResolveKey);
		}

		private readonly Lazy<string> _keyLazy;
		public string Key => this._keyLazy.Value;

		public IMasterDataProvider Create(IServiceProvider serviceProvider)
		{
			return serviceProvider.GetRequiredService<TProvider>();
		}
	}
}
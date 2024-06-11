using FwaEu.Modules.MasterData;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Tests.MasterData
{
	public class MasterDataProviderFactoryStub : IMasterDataProviderFactory
	{
		public MasterDataProviderFactoryStub(string key, IMasterDataProvider provider)
		{
			this.Key = key ?? throw new ArgumentNullException(nameof(key));
			this._provider = provider ?? throw new ArgumentNullException(nameof(provider));
		}

		public string Key { get; }
		private readonly IMasterDataProvider _provider;

		public IMasterDataProvider Create(IServiceProvider serviceProvider)
		{
			return this._provider;
		}
	}
}

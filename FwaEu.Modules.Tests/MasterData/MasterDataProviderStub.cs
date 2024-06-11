using FwaEu.Modules.MasterData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.MasterData
{
	public class MasterDataProviderStub : IMasterDataProvider
	{
		public MasterDataProviderStub(DateTime maximumUpdatedOn, int count, IEnumerable data)
		{
			this._maximumUpdatedOn = maximumUpdatedOn;
			this._count = count;
			this._data = data ?? throw new ArgumentNullException(nameof(data));
		}

		private readonly DateTime _maximumUpdatedOn;
		private readonly int _count;
		private readonly IEnumerable _data;

		public Type IdType => throw new NotImplementedException();

		public Task<MasterDataChangesInfo> GetChangesInfoAsync(MasterDataProviderGetChangesParameters parameters)
		{
			return Task.FromResult(new MasterDataChangesInfo(this._maximumUpdatedOn, this._count));
		}

		public Task<IEnumerable> GetModelsAsync(MasterDataProviderGetModelsParameters parameters)
		{
			return Task.FromResult(this._data);
		}

		public Task<IEnumerable> GetModelsByIdsAsync(MasterDataProviderGetModelsByIdsParameters parameters)
		{
			throw new NotImplementedException();
		}
	}
}

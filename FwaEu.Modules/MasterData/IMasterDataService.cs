using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.MasterData
{
	public interface IMasterDataService
	{
		Task<RelatedMasterDataChangesInfo[]> GetChangesInfosAsync(
			RelatedParameters<MasterDataProviderGetChangesParameters>[] parameters);

		Task<RelatedMasterDataGetModelsResult[]> GetModelsAsync(
			RelatedParameters<MasterDataProviderGetModelsParameters>[] parameters);

		Task<RelatedMasterDataGetModelsResult[]> GetModelsByIdsAsync(
			RelatedParameters<MasterDataProviderGetModelsByIdsParameters>[] parameters);

		Type GetIdType(string key);
	}

	public class RelatedParameters<TParameters> where TParameters : MasterDataProviderParametersBase
	{
		public RelatedParameters(string key, TParameters parameters)
		{
			this.Key = key ?? throw new ArgumentNullException(nameof(key));
			this.Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
		}

		public string Key { get; }
		public TParameters Parameters { get; }
	}

	public class RelatedMasterDataChangesInfo
	{
		public RelatedMasterDataChangesInfo(string key, MasterDataChangesInfo changesInfo)
		{
			this.Key = key ?? throw new ArgumentNullException(nameof(key));
			this.ChangesInfo = changesInfo ?? throw new ArgumentNullException(nameof(changesInfo));
		}

		public string Key { get; }
		public MasterDataChangesInfo ChangesInfo { get; }
	}

	public class RelatedMasterDataGetModelsResult
	{
		public RelatedMasterDataGetModelsResult(string key, IEnumerable values)
		{
			this.Key = key ?? throw new ArgumentNullException(nameof(key));
			this.Values = values ?? throw new ArgumentNullException(nameof(values));
		}

		public string Key { get; }
		public IEnumerable Values { get; }
	}
}

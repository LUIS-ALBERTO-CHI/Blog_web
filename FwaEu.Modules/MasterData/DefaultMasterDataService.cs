using System;
using System.Threading.Tasks;

namespace FwaEu.Modules.MasterData
{
	public class DefaultMasterDataService : IMasterDataService
	{
		public DefaultMasterDataService(MasterDataFactoryCache masterDataFactoryCache, IServiceProvider serviceProvider)
		{
			this._masterDataFactoryCache = masterDataFactoryCache
				?? throw new ArgumentNullException(nameof(masterDataFactoryCache));

			this._serviceProvider = serviceProvider
				?? throw new ArgumentNullException(nameof(serviceProvider));
		}

		private readonly MasterDataFactoryCache _masterDataFactoryCache;
		private readonly IServiceProvider _serviceProvider;

		private IMasterDataProvider CreateProvider(string key)
		{
			var factory = this._masterDataFactoryCache.GetFactory(key);
			if (factory == null)
			{
				throw new MasterDataFactoryNotFoundException($"No master data factory registered with the key: {key}");

			}
			return factory.Create(this._serviceProvider);
		}

		public Type GetIdType(string key)
		{
			return this.CreateProvider(key).IdType;
		}

		public async Task<RelatedMasterDataChangesInfo[]> GetChangesInfosAsync(
			RelatedParameters<MasterDataProviderGetChangesParameters>[] parameters)
		{
			//NOTE: Do not run in parallel because NHibernate cannot handle it https://github.com/nhibernate/nhibernate-core/issues/1642
			var changeInfos = new RelatedMasterDataChangesInfo[parameters.Length];
			for (int i = 0; i < parameters.Length; i++)
			{
				var parameter = parameters[i];
				var result = await this.CreateProvider(parameter.Key).GetChangesInfoAsync(parameter.Parameters);
				changeInfos[i] = new RelatedMasterDataChangesInfo(parameter.Key, result);
			}

			return changeInfos;
		}

		public async Task<RelatedMasterDataGetModelsResult[]> GetModelsAsync(
			RelatedParameters<MasterDataProviderGetModelsParameters>[] parameters)
		{
			//NOTE: Do not run in parallel because NHibernate cannot handle it https://github.com/nhibernate/nhibernate-core/issues/1642
			var changeInfos = new RelatedMasterDataGetModelsResult[parameters.Length];
			for (int i = 0; i < parameters.Length; i++)
			{
				var parameter = parameters[i];
				var result = await this.CreateProvider(parameter.Key).GetModelsAsync(parameter.Parameters);
				changeInfos[i] = new RelatedMasterDataGetModelsResult(parameter.Key, result);
			}

			return changeInfos;
		}

		public async Task<RelatedMasterDataGetModelsResult[]> GetModelsByIdsAsync(RelatedParameters<MasterDataProviderGetModelsByIdsParameters>[] parameters)
		{
			//NOTE: Do not run in parallel because NHibernate cannot handle it https://github.com/nhibernate/nhibernate-core/issues/1642
			var changeInfos = new RelatedMasterDataGetModelsResult[parameters.Length];
			for (int i = 0; i < parameters.Length; i++)
			{
				var parameter = parameters[i];
				var result = await this.CreateProvider(parameter.Key).GetModelsByIdsAsync(parameter.Parameters);
				changeInfos[i] = new RelatedMasterDataGetModelsResult(parameter.Key, result);
			}

			return changeInfos;
		}
	}
}

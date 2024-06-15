using FwaEu.Modules.MasterData;
using FwaEu.Modules.Users.Avatars;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.Users.MasterData
{
	public class ApplicationUserMasterDataProvider : IMasterDataProvider
	{
		public ApplicationUserMasterDataProvider(EntityUserMasterDataProvider inner) : this(inner, null)
		{
		}

		public ApplicationUserMasterDataProvider(
			EntityUserMasterDataProvider inner,
			IAvatarService avatarService)
		{
			this._inner = inner ?? throw new ArgumentNullException(nameof(inner));
			this._avatarService = avatarService;
		}

		private readonly EntityUserMasterDataProvider _inner;
		private readonly IAvatarService _avatarService;

		public Type IdType => this._inner.IdType;

		public async Task<MasterDataChangesInfo> GetChangesInfoAsync(MasterDataProviderGetChangesParameters parameters)
		{
			return await this._inner.GetChangesInfoAsync(parameters);
		}

		private async Task FillAvatarAsync(EntityUserMasterDataModel model)
		{
			if (this._avatarService != null) //NOTE: When no service is configured, AvatarUrl property will be null
			{
				var avatar = await this._avatarService.GetAvatarAsync(model);
				model.AvatarUrl = avatar.Url;
			}
		}

		private async Task<IEnumerable> GetFinalModelsAsync(
			Task<IEnumerable<EntityUserMasterDataModel>> getModelsAsyncTask)
		{
			var models = await getModelsAsyncTask;

			foreach (var model in models)
			{
				await this.FillAvatarAsync(model);
			}

			return models.Select(m =>
				new UserMasterDataModel(m.Id, m.FirstName, m.LastName, m.AvatarUrl));
		}

		public async Task<IEnumerable> GetModelsAsync(MasterDataProviderGetModelsParameters parameters)
		{
			return await this.GetFinalModelsAsync(
				this._inner.GetModelsAsync(parameters));
		}

		public async Task<IEnumerable> GetModelsByIdsAsync(MasterDataProviderGetModelsByIdsParameters parameters)
		{
			return await this.GetFinalModelsAsync(
				this._inner.GetModelsByIdsAsync(parameters));
		}
	}
}

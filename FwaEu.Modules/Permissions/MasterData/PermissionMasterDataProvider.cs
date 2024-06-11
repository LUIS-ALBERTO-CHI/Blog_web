using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Globalization;
using FwaEu.Fwamework.Permissions;
using FwaEu.Fwamework.Temporal;
using FwaEu.Modules.MasterData;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FwaEu.Modules.Permissions.MasterData
{
	public class PermissionMasterDataProvider : IMasterDataProvider
	{
		public PermissionMasterDataProvider(IEnumerable<IPermissionProviderFactory> permissionProviderFactories,
			IPermissionMasterDataDate permissionMasterDataDate)
		{
			this._permissionProviderFactories = permissionProviderFactories
				?? throw new ArgumentNullException(nameof(permissionProviderFactories));

			this._permissionMasterDataDate = permissionMasterDataDate
				?? throw new ArgumentNullException(nameof(permissionMasterDataDate));
		}

		private readonly IEnumerable<IPermissionProviderFactory> _permissionProviderFactories;
		private readonly IPermissionMasterDataDate _permissionMasterDataDate;

		public Type IdType => typeof(string);

		protected IEnumerable<IPermission> GetAllPermissions()
		{
			return this._permissionProviderFactories
				.Select(ppf => ppf.Create())
				.SelectMany(pp => pp.GetPermissions());
		}

		public async Task<MasterDataChangesInfo> GetChangesInfoAsync(MasterDataProviderGetChangesParameters parameters)
		{
			var maximumUpdatedOn = await this._permissionMasterDataDate.GetMaximumUpdatedOnAsync();
			var count = this.GetAllPermissions().Count();

			return new MasterDataChangesInfo(maximumUpdatedOn ?? DateTime.Now, count);
		}

		public Task<IEnumerable> GetModelsAsync(MasterDataProviderGetModelsParameters parameters)
		{
			if (parameters.Search != null)
			{
				throw new NotSupportedException("Search is not supported by permissions master-data.");
			}

			if (parameters.Pagination != null)
			{
				throw new NotSupportedException("Pagination is not supported by permissions master-data.");
			}

			if (parameters.OrderBy != null)
			{
				throw new NotSupportedException("OrderBy is not supported by permissions master-data.");
			}

			var models = this.GetAllPermissions()
				.Select(p => new { p.InvariantId });

			return Task.FromResult((IEnumerable)models);
		}

		public Task<IEnumerable> GetModelsByIdsAsync(MasterDataProviderGetModelsByIdsParameters parameters)
		{
			throw new NotSupportedException(); // NOTE: It's a small master-data, pagination is not useful
		}
	}
}

using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Globalization;
using FwaEu.Modules.MasterData;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FwaEu.Modules.Permissions.ByRole.MasterData
{
	public class RolePermissionMasterDataProvider : EntityMasterDataProvider<RolePermissionEntity, int,
		RolePermissionMasterDataModel, RolePermissionEntityRepository, RolePermissionQueryByIdModel>
	{
		public RolePermissionMasterDataProvider(
			MainSessionContext sessionContext,
			ICulturesService culturesService)
			: base(sessionContext, culturesService)
		{
		}

		protected override Expression<Func<RolePermissionEntity, RolePermissionMasterDataModel>>
			CreateSelectExpression(CultureInfo userCulture, CultureInfo defaultCulture)
		{
			return entity => new RolePermissionMasterDataModel(entity.Role.Id, entity.Permission.InvariantId);
		}
	}

	public class RolePermissionMasterDataModel
	{
		public RolePermissionMasterDataModel(int roleId, string permissionInvariantId)
		{
			this.RoleId = roleId;

			this.PermissionInvariantId = permissionInvariantId
				?? throw new ArgumentNullException(nameof(permissionInvariantId));
		}

		public int RoleId { get; }
		public string PermissionInvariantId { get; }
	}
}

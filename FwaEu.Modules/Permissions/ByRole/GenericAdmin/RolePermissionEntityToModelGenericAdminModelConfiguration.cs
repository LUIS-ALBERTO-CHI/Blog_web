using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Globalization;
using FwaEu.Fwamework.Permissions;
using FwaEu.Modules.GenericAdmin;
using FwaEu.Modules.GenericAdminMasterData;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Permissions.ByRole.GenericAdmin
{
	public class RolePermissionModel
	{
		[Property(IsKey = true, IsEditable = false)]
		public int? Id { get; set; }

		[Required]
		[MasterData("Roles")]
		public int? RoleId { get; set; }

		[Required]
		[MasterData("Permissions")]
		public string PermissionInvariantId { get; set; } 

		[Property(IsEditable = false)]
		[MasterData("Users")]
		public int? UpdatedById { get; set; }

		[Property(IsEditable = false)]
		public DateTime? UpdatedOn { get; set; }
	}

	public class RolePermissionEntityToModelGenericAdminModelConfiguration 
		: EntityToModelGenericAdminModelConfiguration<RolePermissionEntity, int, RolePermissionModel, MainSessionContext>
	{
		public const string Key = nameof(RolePermissionEntity);

		private readonly CurrentUserPermissionService _currentUserPermissionService;

		public RolePermissionEntityToModelGenericAdminModelConfiguration(IServiceProvider serviceProvider, ICulturesService culturesService,
			CurrentUserPermissionService currentUserPermissionService)
			: base(serviceProvider, culturesService)
		{
			_currentUserPermissionService = currentUserPermissionService
				?? throw new ArgumentNullException(nameof(currentUserPermissionService));
		}

		public override async Task<bool> IsAccessibleAsync()
		{
			return await _currentUserPermissionService.HasPermissionAsync<UserRolePermissionProvider>(
				provider => provider.CanAdministrateRoles);
		}

		protected override async Task FillEntityAsync(RolePermissionEntity entity, RolePermissionModel model)
		{
			var session = this.RepositorySession.Session;

			entity.Role = await session.GetAsync<RoleEntity>(model.RoleId);
			entity.Permission = await session.Query<PermissionEntity>()
				.FirstAsync(p=> p.InvariantId == model.PermissionInvariantId);
		}

		protected override async Task<RolePermissionEntity> GetEntityAsync(RolePermissionModel model)
		{
			return await this.GetRepository().GetAsync(model.Id.Value);
		}

		protected override System.Linq.Expressions.Expression<Func<RolePermissionEntity, RolePermissionModel>> GetSelectExpression()
		{
			return entity => new RolePermissionModel
			{
				Id = entity.Id,
				RoleId = entity.Role.Id,
				PermissionInvariantId = entity.Permission.InvariantId,
				UpdatedById = entity.UpdatedBy.Id,
				UpdatedOn = entity.UpdatedOn,
			};
		}

		protected override bool IsNew(RolePermissionModel model)
		{
			return model.Id == null;
		}

		protected override void SetIdToModel(RolePermissionModel model, RolePermissionEntity entity)
		{
			model.Id = entity.Id;
		}
	}
}

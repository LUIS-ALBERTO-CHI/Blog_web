using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Globalization;
using FwaEu.Fwamework.Permissions;
using FwaEu.Modules.GenericAdmin;
using FwaEu.Modules.GenericAdminMasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FwaEu.Modules.Permissions.ByRole.GenericAdmin
{
	public class RoleModel
	{
		[Property(IsKey = true, IsEditable = false)]
		public int? Id { get; set; }

		[Required]
		public string InvariantId { get; set; }

		[Required]
		[LocalizableStringCustomType]
		public Dictionary<string, string> Name { get; set; }

		[Property(IsEditable = false)]
		[MasterData("Users")]
		public int? CreatedById { get; set; }

		[Property(IsEditable = false)]
		public DateTime? CreatedOn { get; set; }

		[Property(IsEditable = false)]
		[MasterData("Users")]
		public int? UpdatedById { get; set; }

		[Property(IsEditable = false)]
		public DateTime? UpdatedOn { get; set; }
	}

	public class RoleEntityToModelGenericAdminModelConfiguration
		: EntityToModelGenericAdminModelConfiguration<RoleEntity, int, RoleModel, MainSessionContext>
	{
		public const string Key = nameof(RoleEntity);

		public RoleEntityToModelGenericAdminModelConfiguration(IServiceProvider serviceProvider, ICulturesService culturesService)
			: base(serviceProvider, culturesService)
		{
		}

		private readonly CurrentUserPermissionService _currentUserPermissionService;

		public RoleEntityToModelGenericAdminModelConfiguration(IServiceProvider serviceProvider, ICulturesService culturesService,
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

		protected override Task FillEntityAsync(RoleEntity entity, RoleModel model)
		{
			entity.InvariantId = model.InvariantId;
			entity.Name = model.Name.ToStringStringDictionary();
			return Task.CompletedTask;
		}

		protected override async Task<RoleEntity> GetEntityAsync(RoleModel model)
		{
			return await this.GetRepository().GetAsync(model.Id.Value);
		}

		protected override Expression<Func<RoleEntity, RoleModel>> GetSelectExpression()
		{
			return entity => new RoleModel
			{
				Id = entity.Id,
				InvariantId = entity.InvariantId,
				Name = entity.Name.ToStringStringDictionary(),
				CreatedById = entity.CreatedBy.Id,
				CreatedOn = entity.CreatedOn,
				UpdatedById = entity.UpdatedBy.Id,
				UpdatedOn = entity.UpdatedOn,
			};
		}

		protected override bool IsNew(RoleModel model)
		{
			return model.Id == null;
		}

		protected override void SetIdToModel(RoleModel model, RoleEntity entity)
		{
			model.Id = entity.Id;
		}
	}
}
